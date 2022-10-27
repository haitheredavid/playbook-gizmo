using System;
using System.Collections.Generic;
using HaiThere.Playbook;
using UnityEngine;
using UnityEngine.Events;


public class GizmoComponentMover : GizmoComponent
{
  [Header("Gizmo Props")]
  public Camera viewer;
  public bool isParented;
  [Range(1f, 100f)] public float movementSpeed = 100f;

  [Header("Handler Props")]
  [SerializeField, Range(0.0f, 1.0f)] float positionOffset = 0.5f;
  [SerializeField, Range(0.0f, 1.0f)] float scaleOffset = 0.5f;
  [SerializeField] Vector3 anchorOffset = new Vector3(0.2f, 0.0f, 0.2f);
  [SerializeField] bool showLine = true;

  [Header("Debug Position")]
  [SerializeField] bool useLocalPos = true;
  [SerializeField] bool logPos = false;

  public GizmoPieceMove activePiece { get; private set; }

  public event UnityAction OnActionComplete;

  public override void Create()
  {
    var prefab = new GameObject().AddComponent<GizmoPieceMove>();

    prefab.GetComponent<MeshFilter>().mesh = PlaybookMeshBuilder.CreatCube(1f);
    prefab.GetComponent<MeshCollider>().sharedMesh = prefab.GetComponent<MeshFilter>().mesh;
    prefab.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Gizmo");

    prefab.showLine = showLine;
    prefab.isLocal = useLocalPos;
    prefab.movementSpeed = movementSpeed;

    // hard coded size since its just using a cube but this should change for a model later
    prefab.transform.localScale = new Vector3(.2f, .2f, .5f);

    foreach (AxisType axis in Enum.GetValues(typeof(AxisType)))
    {
      var instance = Instantiate(prefab, transform);
      instance.name = $"Mover_{axis}";
      instance.transform.localPosition = GetPiecePosition(axis);
      instance.transform.localRotation = Quaternion.Euler(GetPieceRotation(axis));
      instance.axis = axis;
      instance.parent = this;

      instance.OnClick += SetActivePiece;

      instance.Create();

      pieces.Add(instance);
    }

    Destroy(prefab.gameObject);
  }


  void SetActivePiece(GizmoPieceMove piece)
  {
    if (piece == null)
      return;

    if (activePiece != null)
    {
      if (activePiece.Equals(piece))
        return;

      activePiece.OnStart -= StartActionHook;
      activePiece.OnEnd -= EndActionHook;
    }

    activePiece = piece;
    activePiece.OnStart += StartActionHook;
    activePiece.OnEnd += EndActionHook;
  }

  void StartActionHook()
  {
  }

  void EndActionHook()
  {
    activePiece.transform.localPosition = GetPiecePosition(activePiece.axis);
    OnActionComplete?.Invoke();

    if (!isParented)
      MoveToObjectAnchor();
  }

  public Vector3 pointerPos
  {
    get => viewer == null ? Vector3.zero : viewer.ScreenToWorldPoint(Input.mousePosition);
  }

  void MoveToObjectAnchor()
  {
    transform.position = new Vector3(
      activeObj.transform.position.x - activeObj.transform.localScale.x * 0.5f + anchorOffset.x,
      activeObj.transform.position.y - activeObj.transform.localScale.y * 0.5f + anchorOffset.y,
      activeObj.transform.position.z - activeObj.transform.localScale.z * 0.5f + anchorOffset.z
    );
  }

  Vector3 GetPiecePosition(AxisType axis)
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

  Vector3 GetPieceRotation(AxisType axis)
  {
    return axis switch
    {
      AxisType.X => new Vector3(0, -90, -90),
      AxisType.Y => new Vector3(90, 0, 0),
      AxisType.Z => new Vector3(0, 0, -90),
      _ => Vector3.zero
    };
  }

}
