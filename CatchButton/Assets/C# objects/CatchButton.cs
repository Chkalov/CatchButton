using UnityEngine;

[ExecuteInEditMode]
public class CatchButton : MonoBehaviour
{
	#region SERIALIZE FIELDS

	[SerializeField] private Rect activeZoneRect;
	//Rect "чувствительной" зоны.

	[SerializeField] [Range (0, 100)] private float activeZoneOffset;
	//Отступ "во внутрь" "чувствительной" зоны, соответствующий кнопке за которой игрок "охотится".

	[SerializeField] private GUIStyle activeZoneStyle;
	//Стиль для отрисовки "чувствительной" зоны.

	[SerializeField] private GUIStyle buttonStyle;
	//Стиль для отрисовки кнопки.

	[SerializeField] private bool isDebugMode = true;
	//Флажок для переключения режимов работы. По умолчанию "истина", что соответствует отображению элементов режима отладки.

	#endregion

	#region PRIVATE FIELDS


	#endregion

	#region UNITY EVENTS

	private void OnGUI ()
	{
		if (isDebugMode) {
			GUI.Box (activeZoneRect, "", activeZoneStyle);
		}

		if (GUI.Button (GetButtonRect (activeZoneRect, activeZoneOffset), "PRESS ME!", buttonStyle)) {
			//Выйграл!!
			print ("CatchButton " + "button pressed!");
		}

		//var mousePosition = new Vector2 (Input.mousePosition.x, (Screen.height - Input.mousePosition.y));
	 
		//GUI.Button (buttonRect, mousePosition.ToString ());

		//var checkRect = new Rect ((buttonRect.left - buttonOffset), (buttonRect.top - buttonOffset), (buttonRect.width + 2 * buttonOffset), (buttonRect.height + 2 * buttonOffset));

		//CheckMousePosition (mousePosition, checkRect);
	}

	#endregion

	#region PRIVATE METHODS

	private Rect GetButtonRect (Rect activeZoneRect, float offSet)
	{
		return new Rect (activeZoneRect.x + offSet, activeZoneRect.y + offSet, activeZoneRect.width - 2 * offSet, activeZoneRect.height - 2 * offSet);
	}

	private void CheckMousePosition (Vector2 mousePosition, Rect rect)
	{
		if (!IsPointOnRect (mousePosition, rect)) {
			return;
		}

		/*var upDelta = mousePosition.y - rect.y;
		var downDelta = (rect.y + rect.height) - mousePosition.y;

		var leftDelta = mousePosition.x - rect.x;
		var rightDelta = rect.x + rect.width - mousePosition.x;


		if (upDelta < downDelta && upDelta < leftDelta && upDelta < rightDelta) {
			print ("move down");

			buttonRect = new Rect (buttonRect.x, buttonRect.y + upDelta, buttonRect.width, buttonRect.height);
		}

		if (downDelta < upDelta && downDelta < leftDelta && downDelta < rightDelta) {
			print ("move up");

			buttonRect = new Rect (buttonRect.x, buttonRect.y - downDelta, buttonRect.width, buttonRect.height);
		}

		if (leftDelta < upDelta && leftDelta < downDelta && leftDelta < rightDelta) {
			print ("move right");

			buttonRect = new Rect (buttonRect.x + leftDelta, buttonRect.y, buttonRect.width, buttonRect.height);
		}

		if (rightDelta < upDelta && rightDelta < downDelta && rightDelta < leftDelta) {
			print ("move left");

			buttonRect = new Rect (buttonRect.x - rightDelta, buttonRect.y, buttonRect.width, buttonRect.height);
		}*/

	}

	private bool IsPointOnRect (Vector2 point, Rect rect)
	{
		return (point.x >= rect.left && point.x <= rect.left + rect.width) && (point.y >= rect.top && point.y <= rect.top + rect.height);
	}

	#endregion

}
