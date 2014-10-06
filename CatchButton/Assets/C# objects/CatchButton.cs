using UnityEngine;
using System.Threading;

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

	private float upDelta, downDelta, leftDelta, rightDelta, minDelta;
	//приватные переменные для определения дельты смещения

	#endregion

	#region UNITY EVENTS

	private void OnGUI ()//Метод MonoBehaviour, в нем происходит отрисовка GUI контролов. Т.к метод отрабатывает каждый кадр в нем я расположил "основной" программный поток.
	{
		if (isDebugMode) {//если isDebugMode == true то происходит отрисовка активной зоны.
			GUI.Box (activeZoneRect, "", activeZoneStyle);// Активная зона. Отрисовка через GUI.Box.
		}

		if (GUI.Button (GetButtonRect (activeZoneRect, activeZoneOffset), "CATCH ME!", buttonStyle)) {//Проверка нажатия "не уловимой" кнопки. Обратите внимание Rect кнопки пересчитывается от activeZoneRect с отступом activeZoneOffset.
			//Выйграл!! Показать эффект.
			print ("CatchButton " + "Win!");
		}

		CheckMousePosition (GetMousePosition (), activeZoneRect);//Метод проверяющий положение курсора мыши. Метод принимает Vector2 - координаты курсора мыши и Rect "чувствительной" зоны.
		//Обратите внимание я не передаю в этот метод Input.mousePosition, я передаю GetMousePosition (), который пересчитывает координаты курсора мыши в систему "верхнего левого угла" а не "нижнего левого".
	}

	#endregion

	#region PRIVATE METHODS

	private void CheckMousePosition (Vector2 mousePosition, Rect rect)//Проверка положения курсора мыши.
	{
		if (!IsPointOnRect (mousePosition, rect)) {//Если курсор не в "чувствительной" зоне происходит возврат программы в "основной" поток.
			return;//Возврат происходт при помощи оператора return (http://msdn.microsoft.com/ru-ru/library/1h3swy84.aspx).
		}//Все данная конструкция представляет собой некую "закрывашку" которая блокирует выполнение дальнейшего кода если не выполняется необходимое условие. 
		//Данная конструкция очень распространнена из-за своего удобства. Рекомендую взять на заметку.

		//Если курсор мыши в "чувствительной" зоне значит необходимо выполнить следующие действия

		GetDeltas (mousePosition, rect);//Пересчитать дельты по 4-м сторонам.
		MoveButton ();//Переместить кнопку.
	}

	private void GetDeltas (Vector2 mousePosition, Rect rect)//Вычисляю дельты по сторонам. Это 
	{
		upDelta = mousePosition.y - rect.yMin;
		downDelta = rect.yMax - mousePosition.y;
		leftDelta = mousePosition.x - rect.xMin;
		rightDelta = rect.xMax - mousePosition.x;
		minDelta = Mathf.Min (upDelta, downDelta, leftDelta, rightDelta);//Очень полезный метод из математического класса Mathf, позволяет найти наименьшее значение из передаваего набора значений.
		//Обратите внимание в данном случае я использую "перегрузку" Mathf.Min(params float[] values), что позволяет передавать список проверяемых параметров через запятую.
	}

	private void MoveButton ()//В зависимости от того к какой стороне ближе курсор мыши кнопка сдвигается в противоположную сторону. Формально говоря, это реализованно по средствам переопределения activeZoneRect.
	{
		if (minDelta == upDelta) {//Если курсор ближе к "верху" то определяем новый activeZoneRect со здвигом в низ.

			var temp = activeZoneRect.y + upDelta;//Обратите внимание что данная локальная переменная "доступна" только в рамках данного if-а.
			temp = temp > Screen.height - activeZoneRect.height ? Screen.height - activeZoneRect.height : temp;//Проверка выхода за край экрана при помощи тернарного оператора (http://msdn.microsoft.com/ru-ru/library/ty67wk28.aspx).

			activeZoneRect = new Rect (activeZoneRect.x, temp, activeZoneRect.width, activeZoneRect.height);//Формируется новый activeZoneRect, по которому в следующем кадре будет отрисованна кнопка.
			return;//Возврат в "основной" поток.
		}

		if (minDelta == downDelta) {

			var temp = activeZoneRect.y - downDelta;
			temp = temp < 0 ? 0 : temp;

			activeZoneRect = new Rect (activeZoneRect.x, temp, activeZoneRect.width, activeZoneRect.height);
			return;
		}

		if (minDelta == leftDelta) {
					
			var temp = activeZoneRect.x + leftDelta;
			temp = temp > Screen.width - activeZoneRect.width ? Screen.width - activeZoneRect.width : temp;

			activeZoneRect = new Rect (temp, activeZoneRect.y, activeZoneRect.width, activeZoneRect.height);
			return;
		}

		if (minDelta == rightDelta) {

			var temp = activeZoneRect.x - rightDelta;
			temp = temp < 0 ? 0 : temp;

			activeZoneRect = new Rect (temp, activeZoneRect.y, activeZoneRect.width, activeZoneRect.height);
			return;
		}
	}

	private bool IsPointOnRect (Vector2 point, Rect rect)//Проверка нахождения точки в Rect.
	{
		return (point.x >= rect.xMin && point.x <= rect.xMax && point.y >= rect.yMin && point.y <= rect.yMax);
	}

	private Rect GetButtonRect (Rect activeZoneRect, float offSet)//Расчитывают Rect кнопки.
	{
		return new Rect (activeZoneRect.x + offSet, activeZoneRect.y + offSet, activeZoneRect.width - 2 * offSet, activeZoneRect.height - 2 * offSet);
	}

	private Vector2 GetMousePosition ()//Привожу координату У курсора мыши к координатам используемым для activeZoneRect.
	{
		var temp = Input.mousePosition;
		return new Vector2 (temp.x, Screen.height - temp.y);
	}

	#endregion

}
