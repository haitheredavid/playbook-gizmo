using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HaiThere.Playbook
{

  public abstract class GizmoComponent : Gizmo
  {
    [Header("Component Props")]
    public Camera viewer;
    public bool isParented = false;
    public bool isLocal = true;
    public bool showLine = true;
    [SerializeField] protected GameObject obj;

    [Header("Transform Props")]
    [Range(1f, 100f)] public float movementSpeed = 100f;
    public float positionOffset = 0.3f;
    public float scaleOffset = 0.3f;
    public Vector3 anchorOffset = new Vector3(0.0f, 0.0f, 0.0f);

    protected List<GizmoPiece> pieces = new List<GizmoPiece>();

    public GizmoPiece activePiece { get; protected set; }

    public GameObject activeObj
    {
      get => obj;
      set
      {

        if (value == null)
          return;

        obj = value;

        foreach (var piece in pieces)
        {
          piece.obj = value.transform;
        }
      }
    }

    public Vector3 pointerPos
    {
      get => viewer == null ? Vector3.zero : viewer.ScreenToWorldPoint(Input.mousePosition);
    }

    public event UnityAction OnActionComplete;

    public override void SetActive(bool status)
    {
      foreach (var p in pieces)
      {
        p.SetActive(status);
      }
    }
    public override void Create()
    {
      var prefab = BuildPrefab();

      foreach (AxisType axis in Enum.GetValues(typeof(AxisType)))
      {
        var instance = Instantiate(prefab, transform);
        instance.name = $"{axis}";
        instance.transform.localPosition = GetPiecePosition(axis);
        instance.transform.localRotation = Quaternion.Euler(GetPieceRotation(axis));
        instance.axis = axis;
        instance.parent = this;
        instance.OnSet += SetActivePiece;
        instance.Create();

        pieces.Add(instance);
      }

      Destroy(prefab.gameObject);
    }

    protected void SetActivePiece(GizmoPiece piece)
    {
      if (piece == null)
        return;

      if (activePiece != null)
      {
        if (activePiece.Equals(piece))
          return;

        activePiece.OnComplete -= CompleteActionHook;
      }

      activePiece = piece;
      activePiece.OnComplete += CompleteActionHook;
    }

    void CompleteActionHook()
    {
      activePiece.transform.localPosition = GetPiecePosition(activePiece.axis);
      OnActionComplete?.Invoke();

      if (!isParented)
        MoveToObjectAnchor();
    }

    protected virtual void MoveToObjectAnchor()
    {
      transform.position = new Vector3(
        activeObj.transform.position.x - activeObj.transform.localScale.x * 0.5f + anchorOffset.x,
        activeObj.transform.position.y - activeObj.transform.localScale.y * 0.5f + anchorOffset.y,
        activeObj.transform.position.z - activeObj.transform.localScale.z * 0.5f + anchorOffset.z
      );
    }

    protected virtual Vector3 GetPiecePosition(AxisType axis)
    {
      var offset = Mathf.Max(positionOffset, scaleOffset * 0.5f);
      return axis switch
      {
        AxisType.X => new Vector3(offset, 0, 0),
        AxisType.Y => new Vector3(0, offset, 0),
        AxisType.Z => new Vector3(0, 0, offset),
        _ => Vector3.zero
      };
    }

    protected virtual Vector3 GetPieceRotation(AxisType axis)
    {
      return axis switch
      {
        AxisType.X => new Vector3(0, 90, 90),
        AxisType.Y => new Vector3(-90, 0, 0),
        AxisType.Z => new Vector3(0, 0, 90),
        _ => Vector3.zero
      };
    }

    protected virtual Mesh CreateMesh() => Builder.CreatCube(1f);

    protected virtual TPiece BuildPrefab<TPiece>() where TPiece : GizmoPiece
    {
      var prefab = new GameObject().AddComponent<TPiece>();

      prefab.GetComponent<MeshFilter>().mesh = CreateMesh();
      prefab.GetComponent<MeshCollider>().sharedMesh = prefab.GetComponent<MeshFilter>().mesh;
      prefab.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Gizmo");
      prefab.movementSpeed = movementSpeed;
      prefab.isLocal = isLocal;
      prefab.transform.localScale = new Vector3(.2f, .2f, scaleOffset);
      return prefab;
    }
    protected abstract GizmoPiece BuildPrefab();

  }
}
