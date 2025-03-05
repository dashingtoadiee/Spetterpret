using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DuskModules {

	/// <summary> Hides the world collision </summary>
	public class MeshGizmo : MonoBehaviour {

		private static Mesh cubeMesh;
		private static Mesh sphereMesh;

		/// <summary> Mesh type </summary>
		public enum MeshType {
			surface,
			obstacle,
			trigger,
			area
		}
		public MeshType type;

#if UNITY_EDITOR
		/// <summary> Draws mesh gizmo </summary>
		private void OnDrawGizmos() {
			Gizmos.color = Color.white;

			switch (type) {
				case MeshType.surface: Gizmos.color = new Color32(0, 40, 255, 64); break;
				case MeshType.obstacle: Gizmos.color = new Color32(255, 0, 0, 64); break;
				case MeshType.trigger: Gizmos.color = new Color32(255, 128, 0, 64); break;
				case MeshType.area: Gizmos.color = new Color32(255, 255, 0, 64); break;
			}

			if (cubeMesh == null) {
				GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cubeMesh = primitive.GetComponent<MeshFilter>().sharedMesh;
				DestroyImmediate(primitive);
			}
			if (sphereMesh == null) {
				GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphereMesh = primitive.GetComponent<MeshFilter>().sharedMesh;
				DestroyImmediate(primitive);
			}

			MeshFilter mesh = gameObject.GetComponent<MeshFilter>();
			if (mesh.sharedMesh == cubeMesh) {
				Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
				Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
				Gizmos.color = Gizmos.color.WithA(Gizmos.color.a / 4);
				Gizmos.DrawCube(Vector3.zero, Vector3.one);
			}
			else if (mesh.sharedMesh == sphereMesh) {
				Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
				Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
				Gizmos.color = Gizmos.color.WithA(Gizmos.color.a / 4);
				Gizmos.DrawMesh(mesh.sharedMesh, Vector3.zero, Quaternion.identity, Vector3.one);
			}
			else {
				Gizmos.DrawWireMesh(mesh.sharedMesh, transform.position, transform.rotation, transform.lossyScale);
				Gizmos.color = Gizmos.color.WithA(Gizmos.color.a / 4);
				Gizmos.DrawMesh(mesh.sharedMesh, transform.position, transform.rotation, transform.lossyScale);
			}
		}
#endif
	}
}