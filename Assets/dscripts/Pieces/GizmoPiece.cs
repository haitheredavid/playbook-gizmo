using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace HaiThere.Playbook
{
  public enum AxisType
  {
    X,
    Y,
    Z
  }

  [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
  public abstract class GizmoPiece : Gizmo, IDragHandler, IBeginDragHandler, IEndDragHandler
  {

    [SerializeField, HideInInspector] protected MeshFilter meshFilter;
    [SerializeField, HideInInspector] protected MeshRenderer meshRenderer;
    [SerializeField, HideInInspector] protected MeshCollider meshCollider;

    [SerializeField, Range(0.01f, 10f)] protected float precision = 0.5f;

    protected LineRenderer line;
    protected Vector3 objDelta;
    protected Vector3 positionDelta;
    protected Material material;

    public float movementSpeed { get; set; } = 100f;
    public bool isLocal { get; set; } = true;
    public AxisType axis { get; set; }
    public bool showLine { get; set; } = true;
    public Transform obj { get; set; }
    public GizmoComponent parent { get; set; }

    public event UnityAction<GizmoPiece> OnSet;
    public event UnityAction OnComplete;


    public override void SetActive(bool status)
    {
      if (meshRenderer)
        meshRenderer.enabled = status;
      if (meshCollider)
        meshCollider.enabled = status;
      if (line)
        line.enabled = status;
    }

    public bool Equals(GizmoPiece value)
    {
      return value != null && value.GetType() == this.GetType() && value.axis == axis;
    }

    public override void Create()
    {
      meshCollider = gameObject.GetComponent<MeshCollider>();
      meshFilter = gameObject.GetComponent<MeshFilter>();
      meshRenderer = gameObject.GetComponent<MeshRenderer>();
      material = meshRenderer.material;
      material.color = PlaybookColors.GetAxisColor(axis);

      line = gameObject.AddComponent<LineRenderer>();
      line.enabled = showLine;
      line.loop = false;
      line.endWidth = line.startWidth = 0.02f;
      line.material = Resources.Load<Material>("Line");
      line.material.color = material.color;
      line.SetPositions(new[] { Vector3.zero, Vector3.zero });
    }

    protected Vector3 GetDeltaMovementChange(Vector3 pos, Vector3 offset)
    {
      return axis switch
      {
        AxisType.X => new Vector3(pos.x - offset.x, positionDelta.y, positionDelta.z),
        AxisType.Y => new Vector3(positionDelta.x, pos.y - offset.y, positionDelta.z),
        AxisType.Z => new Vector3(positionDelta.x, positionDelta.y, pos.z - offset.z),
        _ => pos - offset
      };
    }


    public virtual void OnDrag(PointerEventData data)
    {
      Vector3 newPos;

      if (data.pointerCurrentRaycast.worldPosition.x == 0.0f || data.pointerCurrentRaycast.worldPosition.y == 0.0f)
      {
        newPos = GetDeltaMovementChange(parent.pointerPos, positionDelta);
      }
      else
      {
        newPos = GetDeltaMovementChange(data.pointerCurrentRaycast.worldPosition, positionDelta);
      }

      Vector3 result;
      if (isLocal)
      {
        result = Vector3.MoveTowards(transform.localPosition, newPos, movementSpeed * Time.deltaTime);
        transform.localPosition = result;
      }
      else
      {
        result = Vector3.MoveTowards(transform.position, newPos, movementSpeed * Time.deltaTime);
        transform.position = result;
      }

      if (showLine)
        SetLinePosFromDrag();

      if (obj != null)
        ApplyResult(result + objDelta);

      Debug.Log($"{name}-Drag");
    }

    public virtual void OnBeginDrag(PointerEventData data)
    {
      positionDelta = GetDeltaMovementChange(data.pointerPressRaycast.worldPosition, isLocal ? transform.localPosition : transform.position);

      if (obj != null)
        objDelta = obj.position - (isLocal ? transform.localPosition : transform.position);

      if (showLine)
        line.SetPosition(0, transform.position);

      OnSet?.Invoke(this);
      Debug.Log($"{name}-Start");
    }

    public virtual void OnEndDrag(PointerEventData data)
    {
      if (showLine)
      {
        line.SetPosition(0, transform.position);
      }

      OnComplete?.Invoke();
      Debug.Log($"{name}-End");
    }

    protected virtual void SetLinePosFromDrag()
    {
      line.SetPosition(1, transform.position);
    }

    protected abstract void ApplyResult(Vector3 result);

    protected static bool ObjectValid<TObj>(TObj mesh) where TObj : UnityEngine.Object
    {
      return mesh != null;
    }
  }
}
