using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using static PetController;
using UnityEngine.InputSystem;

public class PetController : MonoBehaviour
{
    public enum PetHumor { Angry, Happy }
    private GameObject _player;

    [Header("Pet Changing")]
    [SerializeField] private List<GameObject> _petsPrefabs = new List<GameObject>();
    [SerializeField] private int _petID;

    [Header("Following Variables")]
    [SerializeField] private float _followingMaxSpeed;
    [SerializeField] private float _deaceleration;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _maxDistance;

    [Header("Pet Dialogue")]
    [SerializeField] private float _dialogueCooldown;
    [SerializeField] private float _dialogueDelay;

    [SerializeField] private List<string> _petAngryReactions;
    [SerializeField] private List<string> _petHappyReactions;

    [SerializeField] private List<string> _playerAngryAnswers;
    [SerializeField] private List<string> _playerHappyAnswers;

    private ArrayList _petSentences = new ArrayList();

    private ArrayList _playerSentences = new ArrayList();

    private List<TreeNode<DialogueSentence>> _dialogueTrees = new List<TreeNode<DialogueSentence>>();
    

    [Header("UI")]
    [SerializeField] private TMP_Text _text;

    [Header("Public Variables")]
    [HideInInspector] public bool _interacting = false;
    [HideInInspector] public bool _typing = false;
    public PetHumor _petHumor;

    private Rigidbody2D _rb;
    private ParticleSystem _ps;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _ps = GetComponent<ParticleSystem>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Start()
    {
        LoadInteractions();
    }
    void Update()
    {
        if(_text == null)
        {
            foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Pet"))
            {
                if (temp != this.gameObject)
                    _text = temp.GetComponent<TMP_Text>();
            }
        }
        RotatePet();
        if(_dialogueCooldown >= 0)
        {
            _dialogueCooldown -= Time.deltaTime;
        }
        else if(!_interacting)
        {
            StartCoroutine(Dialogue());
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_petID == 0)
                ChangePet(1);
            else if (_petID == 1)
                ChangePet(0);
        }
    }

    void FixedUpdate()
    {
        //Follows the player
        Vector3 direction;
        if (!_player.GetComponent<PlayerController>()._isWallSliding)
        {
            direction = _player.transform.position + new Vector3(_offset.x * -_player.transform.localScale.x, _offset.y, _offset.z) - transform.position;
        }
        else
        {
            direction = _player.transform.position + new Vector3(_offset.x * _player.transform.localScale.x, _offset.y, _offset.z) - transform.position;
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
            _ps.Play();
        }
        else if (_rb.velocity.magnitude < 0.5f)
        {
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
        foreach (string reaction in _petAngryReactions)
        {
            _petSentences.Add(new DialogueSentence(PetHumor.Angry, reaction));
        }
        foreach (string reaction in _petHappyReactions)
        {
            _petSentences.Add(new DialogueSentence(PetHumor.Happy, reaction));
        }

        foreach (string answer in _playerAngryAnswers)
        {
            _playerSentences.Add(new DialogueSentence(PetHumor.Angry, answer));
        }
        foreach (string answer in _playerHappyAnswers)
        {
            _playerSentences.Add(new DialogueSentence(PetHumor.Happy, answer));
        }

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
        TreeNode<DialogueSentence> treeNode = new TreeNode<DialogueSentence>(null, null, null);

        int temp = Random.Range(0, _dialogueTrees.Count);

        while (_dialogueTrees[temp]._data.Humor != _petHumor)
        {
            temp = Random.Range(0, _dialogueTrees.Count);
            yield return null;
        }

        if(_dialogueTrees[temp]._data.Humor == _petHumor)
        {
            treeNode = _dialogueTrees[temp];
            StartCoroutine(Type(treeNode._data.Text, "Pet"));

            yield return new WaitForSeconds(_dialogueDelay);

            int random = Random.Range(0, 2);
            if (random == 0)
            {
                StartCoroutine(Type(treeNode._left._data.Text, "Ryo"));
            }
            else
            {
                StartCoroutine(Type(treeNode._right._data.Text, "Ryo"));
            }

            _dialogueCooldown = Random.Range(50, 90);
        }

        _interacting = false;
        yield return null;
    }
    public IEnumerator Type(string s, string speaker)
    {
        if (!_typing)
        {
            _typing = true;
            _text.text = "";
            string temp = speaker + ": ";

            for (int i = 0; i < s.Length; i++)
            {
                temp += s[i];
                _text.text = temp;
                yield return new WaitForSeconds(0.04f);
            }
            yield return new WaitForSeconds(1f);

            for (int i = temp.Length - 1; i >= 0; i--)
            {
                temp = temp.Remove(i, 1);
                _text.text = temp;
                yield return new WaitForSeconds(0.04f);
            }
            _typing = false;
            yield return null;
        }
        
    }

    void ChangePet(int index)
    {
        _text.text = "";
        Instantiate(_petsPrefabs[index]);
        Destroy(this.gameObject);
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