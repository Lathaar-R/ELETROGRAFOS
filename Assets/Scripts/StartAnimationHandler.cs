using UnityEngine;
using DG.Tweening;

class StartAnimationHandler
{
    LevelType levelType;
    Transform transform;
    RectTransform rectTransform;
    Collider2D collider;
    Vector2 dir;
    Vector2 center;
    Vector2 outsidePos;
    int type = 0;
    public StartAnimationHandler(Transform transform, Collider2D collider, Vector2 dir, LevelType levelType)
    {
        this.transform = transform;
        this.collider = collider;
        this.dir = dir;
        this.center = transform.position;
        this.levelType = levelType;
        CallBackManeger.Instance.onStartLevelAnimation += MoveToCenter;
        CallBackManeger.Instance.onEndLevelAnimation += MoveBack;
        MoveToStart();
        type = 1;
    }

    public StartAnimationHandler(RectTransform transform, Vector2 dir, LevelType levelType)
    {
        this.rectTransform = transform;
        this.dir = dir;
        this.center = transform.anchoredPosition;
        this.levelType = levelType;
        CallBackManeger.Instance.onStartLevelAnimation += MoveToCenterCanvas;
        CallBackManeger.Instance.onEndLevelAnimation += MoveBackCanvas;
        MoveToStartCanvas();
        type = 2;
    }

    ~StartAnimationHandler()
    {
        if (type == 1)
        {
            CallBackManeger.Instance.onStartLevelAnimation -= MoveToCenter;
            CallBackManeger.Instance.onEndLevelAnimation -= MoveBack;
        }
        else if (type == 2)
        {
            CallBackManeger.Instance.onStartLevelAnimation -= MoveToCenterCanvas;
            CallBackManeger.Instance.onEndLevelAnimation -= MoveBackCanvas;
        }
    }

    public void MoveToStart()
    {
        // Get the extents of the screen in world coordinates
        float extent = Vector2.Scale(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)), dir.normalized).magnitude;

        // Calculate the half size of the object
        Vector3 halfSize = collider.bounds.extents;

        // Calculate the vector to move the object to the center of the screen tangent to dir
        transform.position += Vector3.Scale(-center, dir.x == 0 ? new Vector3(0, 1, 0) : new Vector3(1, 0, 0));
        transform.position += (Vector3)dir * extent + Vector3.Scale(halfSize, dir);

        outsidePos = transform.position;
    }

    public void MoveToStartCanvas()
    {
        Vector2 extend = Vector3.Scale(rectTransform.rect.size, dir);

        rectTransform.anchoredPosition += extend;
        outsidePos = rectTransform.anchoredPosition;
    }

    public void MoveBack()
    {
        transform.DOMove(outsidePos, 1f);
    }

    public void MoveToCenter()
    {
        Debug.Log((int)levelType);
        Debug.Log(GameManagerScript.Instance.LevelType);

        Debug.Log(levelType & GameManagerScript.Instance.LevelType);
        if ((levelType & GameManagerScript.Instance.LevelType) > 0)
            transform.DOMove(center, 1f);
        
    }



    public void MoveBackCanvas()
    {
        rectTransform.DOAnchorPos(outsidePos, 1f);
    }

    public void MoveToCenterCanvas()
    {
        if ((levelType & GameManagerScript.Instance.LevelType) > 0)
            rectTransform.DOAnchorPos(center, 1f);
    }


}