using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotation : MonoBehaviour {

	public Material annoMat;
	public float lineWidth = 0.08f;
	public float minVertDist=0.1f;

	bool touchdown=false;
	bool drawLine=false;
	Vector2 touchStart;
	Vector2 touchCurr;
	Vector2 touchLast;
	Touch touch0;

	GameObject currLine;
	Vector3[] currPos;
	List<GameObject> annotationList = new List<GameObject>();
	int annCounter=0;
	int vertCounter=0;

	void Update()
	{
		if (Input.touchCount > 0) {
			touch0 = Input.touches [0];
			if (touch0.phase == TouchPhase.Began) {
				touchdown = true;
				touchStart = touch0.position;
			} else if (touch0.phase == TouchPhase.Ended) {
				touchdown = false;
				drawLine = false;
			}

			touchCurr = touch0.position;

			if (touchdown && !drawLine && touch0.phase == TouchPhase.Moved) {				

				if ((touchStart - touchCurr).sqrMagnitude > 0.5f) {
					drawLine = true;
					GameObject go = new GameObject ("ann" + annCounter.ToString ());
					go.transform.SetParent (transform);
					go.AddComponent<LineRenderer> ().numPositions = annCounter;
					go.GetComponent<LineRenderer> ().material = annoMat;
					go.GetComponent<LineRenderer> ().startWidth = go.GetComponent<LineRenderer> ().endWidth = lineWidth;
					go.GetComponent<LineRenderer> ().widthMultiplier = lineWidth;
					go.GetComponent<LineRenderer> ().receiveShadows = false;
					go.GetComponent<LineRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
					currLine = go;
					annotationList.Add (go);
					annCounter++;
					vertCounter = 0;
					touchLast = touchCurr;
				}
			}

			if (drawLine && (touchCurr-touchLast).sqrMagnitude>minVertDist) {
				vertCounter++;
				Vector3 worldPos = Camera.main.ScreenToWorldPoint (new Vector3 (touch0.position.x, touch0.position.y, 2f));
				currLine.GetComponent<LineRenderer> ().numPositions = vertCounter;
				currLine.GetComponent<LineRenderer> ().SetPosition (vertCounter-1, worldPos);
				touchLast = touchCurr;
			}

		}//if touchcount>0


	}//end update

	public void UndoAnnotation()
	{
		Destroy (annotationList [annCounter - 1]);
		annotationList.RemoveAt (annCounter - 1);
		annCounter--;
	}
}
