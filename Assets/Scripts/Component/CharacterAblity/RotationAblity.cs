using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAblity : Ablities
{
    [SerializeField] private Book bookPrefab;
    private List<Book> bookList;
    protected override void Start()
    {
        bookList = new List<Book>();
        base.Start();
    }
    private float distanceToPlayer = 2f;
    private float rotationTime = 4;
    private float stepTime = 5;
    private float speed = 4;
    private bool isAblitying = false;
    private float timeCounter=0;
    private void FixedUpdate()
    {
        if (isAblitying)
        {
            RotateBooks();
        }
    }
    private void RotateBooks()
    {
        timeCounter += Time.fixedDeltaTime;
        for (int i = 0; i < bookList.Count; i++)
        {
            float angle = (timeCounter * speed + (2 * Mathf.PI) * i / bookList.Count);
            float realAngle = angle % (2 * Mathf.PI);
            float offSetX = distanceToPlayer * Mathf.Cos(realAngle);
            float x = transform.position.x - offSetX;
            float offSetY = distanceToPlayer * Mathf.Sin(realAngle);
            float y = transform.position.y - offSetY;
            bookList[i].transform.position = new Vector2(x,y);
            bookList[i].gameObject.SetActive(true);
        }
    }
    private Coroutine corRotateBook;
    private IEnumerator IERotateBook()
    {
        isAblitying = true;
        yield return new WaitForSeconds(rotationTime);
        StopDoAblities();
        yield return new WaitForSeconds(stepTime);
        StartCoroutine(IERotateBook());
    }
    public override void DoAblities()
    {
        if (isStartDoing)
        {
            for(int i = 0; i < level; i++)
            {
                Book book = Instantiate(bookPrefab, transform);
                book.SetDame(level);
                bookList.Add(book);
            }
            isStartDoing = false;
        }
        else
        {
            level++;
            for (int i = 0; i < bookList.Count; i++)
            {
                bookList[i].SetDame(level);
            }
            Book book = Instantiate(bookPrefab, transform);
            book.SetDame(level);
            bookList.Add(book);
        }
        if (level <= 0) return;
        if (corRotateBook != null)
        {
            StopCoroutine(corRotateBook);
        }
        corRotateBook = StartCoroutine(IERotateBook());
    }
    private void StopDoAblities()
    {
        isAblitying = false;
        foreach (Book buk in bookList)
        {
            buk.gameObject.SetActive(false);
        }
    }
}
