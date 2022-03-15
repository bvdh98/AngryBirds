using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Vector2 _startPos;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 5;

    public bool IsDragging { get; private set; }

    // Start is called before the first frame update

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        _startPos = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
        
    }

    void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;
        IsDragging = true;
    }

    void OnMouseUp()
    {
        IsDragging = false;
        var currentPos = _rigidbody2D.position;
        var direction = _startPos - currentPos;
        direction.Normalize();
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _launchForce);

        _spriteRenderer.color = Color.white;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 desiredPos = mousePos;

        float distance = Vector2.Distance(desiredPos, _startPos);
        if(distance > _maxDragDistance)
        {
            Vector2 direction = desiredPos - _startPos;
            direction.Normalize();
            desiredPos = _startPos + (direction * _maxDragDistance);
        }

        if (desiredPos.x > _startPos.x)
            desiredPos.x = _startPos.x;

        _rigidbody2D.position = desiredPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);
        _rigidbody2D.position = _startPos;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
