using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using LuaInterface;

[AutoRegistLua]
public class DoTween
{
    private Tweener tweener;
    private Sequence sequence;
    private Tween current;
    private GameObject currentGo;

    private DoTween(Sequence sequence, GameObject go)
    {
        this.sequence = sequence;
        current = sequence;
        currentGo = go;
    }
    private DoTween(Tweener tweener, GameObject go)
    {
        this.tweener = tweener;
        current = tweener;
        currentGo = go;
        this.Ease(1);
    }
    public static DoTween Sequence(GameObject go)
    {
        return new DoTween(DOTween.Sequence(), go);
    }
    public DoTween Append(DoTween tweener)
    {
        sequence.Append(tweener.current);
        return this;
    }
    public DoTween Insert(DoTween tweener)
    {
        sequence.Insert(0, tweener.current);
        return this;
    }

    public bool IsComplete()
    {
        return current.IsComplete();
    }
    public bool IsPlaying()
    {
        return current.IsPlaying();
    }

    public void OnCallFunc(LuaFunction func)
    {
        if (currentGo == null || currentGo.Equals(null)) { return; }
        func.Call(this);
    }

    public DoTween OnUpdate(LuaFunction func)
    {
        current.OnUpdate(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween OnComplete(LuaFunction func)
    {
        current.OnComplete(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween OnStepComplete(LuaFunction func)
    {
        current.OnStepComplete(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween OnStart(LuaFunction func)
    {
        current.OnStart(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween OnKill(LuaFunction func)
    {
        current.OnKill(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween OnPause(LuaFunction func)
    {
        current.OnPause(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween OnPlay(LuaFunction func)
    {
        current.OnPlay(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween OnRewind(LuaFunction func)
    {
        current.OnRewind(delegate () { OnCallFunc(func); });
        return this;
    }
    public DoTween Delay(float delay)
    {
        current.SetDelay(delay);
        return this;
    }
    public DoTween From()
    {
        tweener.From();
        return this;
    }
    public DoTween Complete()
    {
        current.Complete();
        return this;
    }
    public DoTween Rewind()
    {
        current.Rewind();
        return this;
    }
    public DoTween Loop(int loops)
    {
        return Loop(loops, 0);
    }
    public DoTween Loop(int loops, uint type)
    {
        if (sequence != null)
        {
            sequence.SetLoops(loops, (LoopType)type);
        }
        if (tweener != null)
        {
            tweener.SetLoops(loops, (LoopType)type);
        }
        return this;
    }
    public DoTween Kill(bool complete)
    {
        current.Kill(complete);
        return this;
    }
    public DoTween Play()
    {
        current.Play();
        return this;
    }
    public DoTween Pause()
    {
        current.Pause();
        return this;
    }
    public DoTween Ease(uint ease)
    {
        current.SetEase((Ease)ease);
        return this;
    }
    public DoTween SetAutoKill(bool bAuto)
    {
        current.SetAutoKill(bAuto);
        return this;
    }
    public DoTween Restart()
    {
        current.Restart();
        return this;
    }
    public static DoTween NoneTo(GameObject go, float duration)
    {
        return new DoTween(DOTween.To(() => 0, v => v = 0, 0, duration + 0.00001f), go);
    }

    public static DoTween To(GameObject go, int startValue, int endValue, float duration, LuaFunction setter)
    {
        int currValue = startValue;
        return new DoTween(DOTween.To(() => startValue, delegate (int v)
        {
            if (currValue != v)
            {
                currValue = v;
                if (go != null)
                {
                    setter.Call(v);
                }
            }
        }, endValue, duration + 0.00001f), go);
    }

   // punch
   /**
   **   第一个参数 punch：表示方向及强度
        第二个参数 duration：表示动画持续时间
        第三个参数 vibrato：震动次数
        第四个参数 elascity: 这个值是0到1的
        当为0时，就是在起始点到目标点之间运动
        不为0时，会把你赋的值乘上一个参数，作为你运动方向反方向的点，物体在这个点和目标点之间运动
   ***/
    public static DoTween DOPunchPosition(GameObject go, Vector3 punch, float duration,int vibrato=10,float elascity=1)
    {
        return new DoTween(go.GetComponent<Transform>().DOPunchPosition(punch, duration + 0.00001f,vibrato,elascity), go);
    }
    public static DoTween DOPunchScale(GameObject go, Vector3 punch, float duration,int vibrato=10,float elascity=1)
    {
        return new DoTween(go.GetComponent<Transform>().DOPunchScale(punch, duration + 0.00001f,vibrato,elascity), go);
    }
    public static DoTween DOPunchRotation(GameObject go, Vector3 punch, float duration,int vibrato=10,float elascity=1)
    {
        return new DoTween(go.GetComponent<Transform>().DOPunchRotation(punch, duration + 0.00001f,vibrato,elascity), go);
    }

    public static DoTween MoveTo(GameObject go, Vector3 pos, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DOMove(pos, duration + 0.00001f), go);
    }

    public static DoTween MoveBy(GameObject go, Vector3 pos, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DOBlendableMoveBy(pos, duration + 0.00001f), go);
    }

    public static DoTween RotateTo(GameObject go, Vector3 angle, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DORotate(angle, duration + 0.00001f, RotateMode.Fast), go);
    }

    public static DoTween RotateBy(GameObject go, Vector3 angle, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DOBlendableRotateBy(angle, duration + 0.00001f, RotateMode.Fast), go);
    }

    public static DoTween RotateModeTo(GameObject go, Vector3 angle, int rotateMode, float duration)
    {
        RotateMode mode = (RotateMode)rotateMode;
        return new DoTween(go.GetComponent<Transform>().DORotate(angle, duration + 0.00001f, mode), go);
    }

    public static DoTween RotateModeBy(GameObject go, Vector3 angle, int rotateMode, float duration)
    {
        RotateMode mode = (RotateMode)rotateMode;
        return new DoTween(go.GetComponent<Transform>().DOBlendableRotateBy(angle, duration + 0.00001f, mode), go);
    }

    public static DoTween ScaleTo(GameObject go, Vector3 scale, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DOScale(scale, duration + 0.00001f), go);
    }

    public static DoTween ScaleBy(GameObject go, Vector3 scale, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DOBlendableScaleBy(scale, duration + 0.00001f), go);
    }

    public static DoTween FadeTo(GameObject go, float alpha, float duration)
    {
        Graphic graphic = go.GetComponent<Graphic>();
        if (graphic != null)
        {
            return new DoTween(graphic.DOFade(alpha, duration + 0.00001f), go);
        }
        return NoneTo(go, duration);
    }

    public static DoTween FadeToGroup(GameObject go, float alpha, float duration)
    {
        CanvasGroup canvasGroup = go.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = go.AddComponent<CanvasGroup>();
        }
        return new DoTween(canvasGroup.DOFade(alpha, duration + 0.00001f), go);
    }

    public static DoTween ColorTo(GameObject go, Color color, float duration)
    {
        Graphic graphic = go.GetComponent<Graphic>();
        if (graphic != null)
        {
            return new DoTween(graphic.DOColor(color / 255, duration + 0.00001f), go);
        }
        return NoneTo(go, duration);
    }

    public static DoTween ColorToAll(GameObject go, Color color, float duration)
    {
        var tween = ColorTo(go, color, duration);
        tween.current.OnStart(() =>
        {
            Graphic[] graphics = go.GetComponentsInChildren<Graphic>();
            foreach (var g in graphics)
            {
                g.CrossFadeColor(color, duration, false, true);
            }
        });
        return tween;
    }
   public static DoTween LocalMoveTo(GameObject go, Vector3 pos, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DOLocalMove(pos, duration + 0.00001f), go);
    }

    public static DoTween LocalMoveBy(GameObject go, Vector3 pos, float duration)
    {
        return new DoTween(go.GetComponent<Transform>().DOBlendableLocalMoveBy(pos, duration + 0.00001f), go);
    }

    public static DoTween AnchorPosTo(GameObject go, Vector2 pos, float duration)
    {
        return new DoTween(go.GetComponent<RectTransform>().DOAnchorPos(pos, duration + 0.00001f), go);
    }

    public static DoTween AnchorPosBy(GameObject go, Vector2 pos, float duration)
    {
        RectTransform rTransform = go.GetComponent<RectTransform>();
        pos = pos + rTransform.anchoredPosition;
        return new DoTween(rTransform.DOAnchorPos(pos, duration + 0.00001f), go);
    }

    public static DoTween AnchorPosXTo(GameObject go, float x, float duration)
    {
        return new DoTween(go.GetComponent<RectTransform>().DOAnchorPosX(x, duration + 0.00001f), go);
    }

    public static DoTween AnchorPosXBy(GameObject go, float x, float duration)
    {
        RectTransform rTransform = go.GetComponent<RectTransform>();
        x = x + rTransform.anchoredPosition.x;
        return new DoTween(rTransform.DOAnchorPosX(x, duration + 0.00001f), go);
    }

    public static DoTween AnchorPosYTo(GameObject go, float y, float duration)
    {
        return new DoTween(go.GetComponent<RectTransform>().DOAnchorPosY(y, duration + 0.00001f), go);
    }

    public static DoTween AnchorPosYBy(GameObject go, float y, float duration)
    {
        RectTransform rTransform = go.GetComponent<RectTransform>();
        y = y + rTransform.anchoredPosition.y;
        return new DoTween(rTransform.DOAnchorPosY(y, duration + 0.00001f), go);
    }

    public static DoTween PreferredSizeTo(GameObject go, Vector2 size, float duration, bool snapping)
    {
        return new DoTween(go.GetComponent<LayoutElement>().DOPreferredSize(size, duration + 0.00001f), go);
    }

    public static DoTween PreferredSizeBy(GameObject go, Vector2 size, float duration, bool snapping)
    {
        LayoutElement layoutElement = go.GetComponent<LayoutElement>();
        size = size + new Vector2(layoutElement.preferredWidth, layoutElement.preferredHeight);
        return new DoTween(layoutElement.DOPreferredSize(size, duration + 0.00001f), go);
    }

    public static DoTween HorizontalNormalizedPosBy(GameObject go, float byValue, float duration, bool snapping = false)
    {
        ScrollRect rect = go.GetComponent<ScrollRect>();
        Vector2 normalizdPos = rect.normalizedPosition;
        float endValue = normalizdPos.x + byValue;
        return new DoTween(rect.DOHorizontalNormalizedPos(endValue, duration, snapping), go);
    }

    public static DoTween HorizontalNormalizedPosTo(GameObject go, float endValue, float duration, bool snapping = false)
    {
        ScrollRect rect = go.GetComponent<ScrollRect>();
        return new DoTween(rect.DOHorizontalNormalizedPos(endValue, duration, snapping), go);
    }

    public static DoTween VerticalNormalizedPosBy(GameObject go, float byValue, float duration, bool snapping = false)
    {
        ScrollRect rect = go.GetComponent<ScrollRect>();
        Vector2 normalizdPos = rect.normalizedPosition;
        float endValue = normalizdPos.y + byValue;
        return new DoTween(rect.DOVerticalNormalizedPos(endValue, duration, snapping), go);
    }

    public static DoTween VerticalNormalizedPosTo(GameObject go, float endValue, float duration, bool snapping = false)
    {
        ScrollRect rect = go.GetComponent<ScrollRect>();
        return new DoTween(rect.DOVerticalNormalizedPos(endValue, duration, snapping), go);
    }

    public static DoTween NormalizedPosBy(GameObject go, Vector2 byValue, float duration, bool snapping = false)
    {
        ScrollRect rect = go.GetComponent<ScrollRect>();
        Vector2 endValue = rect.normalizedPosition + byValue;
        return new DoTween(rect.DONormalizedPos(endValue, duration, snapping), go);
    }

    public static DoTween NormalizedPosTo(GameObject go, Vector2 endValue, float duration, bool snapping = false)
    {
        ScrollRect rect = go.GetComponent<ScrollRect>();
        return new DoTween(rect.DONormalizedPos(endValue, duration, snapping), go);
    }
}
