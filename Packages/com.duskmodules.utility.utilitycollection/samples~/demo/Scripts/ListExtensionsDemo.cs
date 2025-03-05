using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Demo script showcasing what is possible with list extension methods </summary>
public class ListExtensionsDemo : MonoBehaviour {

  public SimpleSpriteButton buttonShuffle;
  public SimpleSpriteButton buttonTagRandom;
  public SimpleSpriteButton buttonTagRandomMultiple;
  public SimpleSpriteButton buttonMoveRandom;
  public SimpleSpriteButton buttonMoveRandomMultiple;
  public SimpleSpriteButton buttonReset;

  public Transform listItemContainer;

  public float listOffset = 1;
  public float moveSpeed;
  public float scaleSpeed;
  public float taggedScale = 0.75f;

  private List<Transform> listA;
  private List<Transform> listB;

  private List<Transform> tagged;

  /// <summary> Awakes and connects to buttons </summary>
  private void Awake() {
		buttonShuffle.onInteract += Shuffle;
		buttonTagRandom.onInteract += TagRandom;
		buttonTagRandomMultiple.onInteract += TagRandomMultiple;
		buttonMoveRandom.onInteract += MoveRandom;
		buttonMoveRandomMultiple.onInteract += MoveRandomMultiple;
		buttonReset.onInteract += ReturnAll;

		SetupLists();
  }

  /// <summary> On destroy, remove button hooks to prevent null references </summary>
  private void OnDestroy() {
		buttonShuffle.onInteract -= Shuffle;
		buttonTagRandom.onInteract -= TagRandom;
		buttonTagRandomMultiple.onInteract -= TagRandomMultiple;
		buttonMoveRandom.onInteract -= MoveRandom;
		buttonMoveRandomMultiple.onInteract -= MoveRandomMultiple;
		buttonReset.onInteract -= ReturnAll;
  }

  /// <summary> Initializes the lists and fills them with objects </summary>
  private void SetupLists() {
		tagged = new List<Transform>();
		listA = new List<Transform>();
		listB = new List<Transform>();

    for (int i = 0; i < listItemContainer.childCount; i++)
			listA.Add(listItemContainer.GetChild(i));
  }

  /// <summary> Shuffles listA </summary>
  private void Shuffle() {
		listA.Shuffle();
  }

  /// <summary> Tags a random item from listA </summary>
  private void TagRandom() {
    if (listA.Count == 0) return;

		tagged = new List<Transform>();
    Transform picked = listA.GetRandom();
		tagged.Add(picked);
  }

  /// <summary> Tags random half of listA </summary>
  private void TagRandomMultiple() {
    if (listA.Count == 0) return;

		tagged = new List<Transform>();
    List<Transform> picked = listA.GetRandomCollection(listA.Count / 2);
		tagged.AddRange(picked);
  }

  /// <summary> Moves a random item from listA to listB </summary>
  private void MoveRandom() {
    if (listA.Count == 0) return;

    Transform picked = listA.RemoveRandom();
		listB.Add(picked);
  }

  /// <summary> Moves random half of listA to listB </summary>
  private void MoveRandomMultiple() {
    if (listA.Count == 0) return;

    List<Transform> picked = listA.RemoveRandomCollection(listA.Count / 2);
		listB.AddRange(picked);
  }

  /// <summary> Brings all items to listA and resets tagged </summary>
  private void ReturnAll() {
		listA.AddRange(listB);
		listB = new List<Transform>();
		tagged = new List<Transform>();
  }

  /// <summary> Moves position of list items to correct list. Scales depending on tag. </summary>
  private void Update() {
    for (int i = 0; i < listA.Count; i++) {
      Vector2 targetPosition = listItemContainer.position.XY() + new Vector2(-1, i * -0.5f);
			Vector2 pos = Vector2.MoveTowards(listA[i].position.XY(), targetPosition, moveSpeed * Time.deltaTime);
			listA[i].position = pos;

      Vector3 desiredScale = Vector3.one * ((tagged.Contains(listA[i])) ? taggedScale : 1);
			if (listA[i].localScale != desiredScale)
				listA[i].localScale = Vector3.MoveTowards(listA[i].localScale, desiredScale, scaleSpeed * Time.deltaTime);
    }

    for (int i = 0; i < listB.Count; i++) {
      Vector2 targetPosition = listItemContainer.position.XY() + new Vector2(1, i * -0.5f);
      Vector2 pos = Vector2.MoveTowards(listB[i].position.XY(), targetPosition, moveSpeed * Time.deltaTime);
			listB[i].position = pos;

      Vector3 desiredScale = Vector3.one * ((tagged.Contains(listB[i])) ? taggedScale : 1);
      if (listB[i].localScale != desiredScale)
				listB[i].localScale = Vector3.MoveTowards(listB[i].localScale, desiredScale, scaleSpeed * Time.deltaTime);
    }
    
  }
}
