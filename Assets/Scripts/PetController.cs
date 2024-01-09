using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using static PetController;
using UnityEngine.InputSystem;

public class PetController : MonoBehaviour
{
    public static PetController _instance;
    public enum PetHumor { Happy, Sad }
    [Header("Components")]
    private GameObject _player;

    [Header("Following Variables")]
    [SerializeField] private float _followingMaxSpeed;
    [SerializeField] private float _deaceleration;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _maxDistance;

    [Header("Pet Dialogue")]
    [SerializeField] private float _dialogueCooldown;
    [SerializeField] private float _dialogueDelay;

    [SerializeField] private List<string> _petSadReactions;
    [SerializeField] private List<string> _petHappyReactions;

    [SerializeField] private List<string> _playerSadAnswers;
    [SerializeField] private List<string> _playerHappyAnswers;

    private ArrayList _petSentences = new ArrayList();
    private ArrayList _playerSentences = new ArrayList();

    private List<TreeNode<DialogueSentence>> _dialogueTrees = new List<TreeNode<DialogueSentence>>();

    [Header("Public Variables")]
    [HideInInspector] public bool _interacting = false;
    public int _petID;
    public PetHumor _petHumor;

    private Rigidbody2D _rb;
    private ParticleSystem _ps;
    private PlayerController _playerController;
    private SpriteRenderer _playerSpriteRenderer;
    private Animator _anim;
    private void Start()
    {
        //Get the components
        _rb = GetComponent<Rigidbody2D>();
        _ps = GetComponent<ParticleSystem>();
        _anim = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();

        LoadInfo();

        LoadInteractions();
        TeleportPetToPlayer();
    }
    void Update()
    {
        //Rotate the pet according to player's direction and state
        RotatePet();

        //Calls the dialogue
        if(_dialogueCooldown >= 0)
        {
            _dialogueCooldown -= Time.deltaTime;
        }
        else if(!_interacting)
        {
            StartCoroutine(Dialogue());
        }
    }

    void FixedUpdate()
    {
        transform.localScale = new Vector3(0.7401682f, 0.7312546f, 1);
        //Follows the player
        Vector3 direction;
        if (!_playerController._isWallSliding)
        {
            if (_playerSpriteRenderer.flipX == false)
                direction = _player.transform.position + new Vector3(_offset.x * -1, _offset.y, _offset.z) - transform.position;
            else
                direction = _player.transform.position + new Vector3(_offset.x * 1, _offset.y, _offset.z) - transform.position;
        }
        else
        {
            if (_playerSpriteRenderer.flipX == false)
                direction = _player.transform.position + new Vector3(_offset.x * 1, _offset.y, _offset.z) - transform.position;
            else
                direction = _player.transform.position + new Vector3(_offset.x * -1, _offset.y, _offset.z) - transform.position;
        }
        
        float distance = direction.magnitude;

        float actualSpeed = Mathf.Lerp(0, _followingMaxSpeed, distance / _deaceleration);

        Vector3 velocity = direction.normalized * actualSpeed;
        _rb.velocity = velocity;

        //Max Distance Verifying
        if(distance > _maxDistance)
        {
            TeleportPetToPlayer();
        }

        //Particle System
        if (_rb.velocity.magnitude > 0.5f && !_ps.isPlaying)
        {
            _anim.SetBool("Walk", true);
            _ps.Play();
        }
        else if (_rb.velocity.magnitude < 0.5f)
        {
            _anim.SetBool("Walk", false);
            _ps.Stop();
        }
    }
    private void RotatePet()
    {
        //Rotate the pet
        if (_player.transform.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(0.4f, 0.4f, 1);
        }
        else
        {
            transform.localScale = new Vector3(-0.4f, 0.4f, 1);
        }
    }
    public void TeleportPetToPlayer()
    {
        transform.position = _player.transform.position + _offset;
    }
    private void LoadInteractions()
    {
        //Load the pet's angry reactions
        foreach (string reaction in _petSadReactions)
        {
            _petSentences.Add(new DialogueSentence(PetHumor.Sad, reaction));
        }
        //Load the pet's happy reactions
        foreach (string reaction in _petHappyReactions)
        {
            _petSentences.Add(new DialogueSentence(PetHumor.Happy, reaction));
        }
        //Load the players's angry reactions
        foreach (string answer in _playerSadAnswers)
        {
            _playerSentences.Add(new DialogueSentence(PetHumor.Sad, answer));
        }
        //Load the player's happy reactions
        foreach (string answer in _playerHappyAnswers)
        {
            _playerSentences.Add(new DialogueSentence(PetHumor.Happy, answer));
        }

        //Create a Tree for each combination of dialogues
        foreach (DialogueSentence petSentence in _petSentences)
        {
            DialogueSentence[] temp = new DialogueSentence[3];
            temp[0] = petSentence;

            foreach (DialogueSentence playerSentence in _playerSentences)
            {
                if(playerSentence != null && playerSentence.Humor == petSentence.Humor)
                {
                    if (temp[1] == null) { temp[1] = playerSentence; }
                    else 
                    { 
                        temp[2] = playerSentence; 
                        _dialogueTrees.Add(new TreeNode<DialogueSentence>(temp[0],
                            new TreeNode<DialogueSentence>(temp[1], null, null),
                            new TreeNode<DialogueSentence>(temp[2], null, null))
                            );
                    }
                }
            }
        }
    }
    public IEnumerator Dialogue()
    {
        _interacting = true;
        //Create a new node
        TreeNode<DialogueSentence> treeNode = new TreeNode<DialogueSentence>(null, null, null);

        //Take a random tree node
        int temp = Random.Range(0, _dialogueTrees.Count);

        //Verifys if the selected node has the same humor as the pet
        while (_dialogueTrees[temp]._data.Humor != _petHumor)
        {
            temp = Random.Range(0, _dialogueTrees.Count);
            yield return null;
        }

        //Load the node info
        treeNode = _dialogueTrees[temp];
        //Type the pet's reaction
        StartCoroutine(FindAnyObjectByType<GameManager>().Type(treeNode._data.Text, "Pet"));

        //Wait for the delay of the response
        yield return new WaitForSeconds(_dialogueDelay);

        //Take a random response from the player
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            StartCoroutine(FindAnyObjectByType<GameManager>().Type(treeNode._left._data.Text, "Ryo"));
        }
        else
        {
            StartCoroutine(FindAnyObjectByType<GameManager>().Type(treeNode._right._data.Text, "Ryo"));
        }

        //Set the cooldown of the dialogue as random also
        _dialogueCooldown = Random.Range(50, 90);

        _interacting = false;
        yield return null;
    }

    private void LoadInfo()
    {
        if(LoadingData.CurrentPet.humor == "Happy")
        {
            _petHumor = PetHumor.Happy;
        }
        else
        {
            _petHumor = PetHumor.Sad;
        }
    }
}

public class DialogueSentence
{
    private PetHumor humor;
    private string text;
    public DialogueSentence(PetHumor _humor, string _text) 
    { 
        this.humor = _humor;
        this.text = _text;
    }

    public PetHumor Humor
    {
        get { return humor; }
        set { humor = value; }
    }

    public string Text
    {
        get { return text; }
        set { text = value; }
    }
}