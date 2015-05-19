/*
 * Handles navigation in the GUI using keyboard, mouse or Emotiv headset.
 * up = emotiv right
 * down = emotiv left
 * enter = emotiv push
 * toggle gui = emotiv pull
 * */

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
		public InputHandler InputHandler;
		public EventSystem Eventsystem;
		List<Selectable> Buttons = new List<Selectable> ();
		Selectable SelectedButton;
		int SelectedButtonIndex = 0;
		public Selectable Dragrace, Labyrinth, Ramps, Navigation, Streets;
		public float CogPull, CogPush, CogLeft, CogRight;
		public float v;
		float PreV;
		public float SubmitTresholdHigh, SubmitTresholdLow, AutoScrollTime;
		float ScrollTimer;
		bool CanToggleGUI = true;
		bool PreIsGuiActive = true;
		bool CanSubmit = true;
		bool IsGuiActive;
	
		void Start ()
		{
				SubmitTresholdHigh = 0.5f;
				SubmitTresholdLow = 0.2f;
				AutoScrollTime = 0.8f;
				Buttons.Add (Dragrace);
				Buttons.Add (Labyrinth);
				Buttons.Add (Ramps);
				Buttons.Add (Navigation);
				Buttons.Add (Streets);
				SelectedButton = Buttons [SelectedButtonIndex]; //initialize position in button array
				Eventsystem.SetSelectedGameObject (SelectedButton.gameObject, new BaseEventData (Eventsystem));
		}

		void Update ()
		{		//get cognitiv values
				CogPush = EmoCognitiv.CognitivActionPower [1]; 	//push
				CogPull = EmoCognitiv.CognitivActionPower [2]; 	//pull
				CogLeft = EmoCognitiv.CognitivActionPower [5]; 	//left
				CogRight = EmoCognitiv.CognitivActionPower [6]; //right 
				GUIActive (); //decide if gui is active
				Interactable (); //set buttons interactable if gui is active
				if (IsGuiActive)
						CheckInput (); //recieve input from emotiv and keyboard
		}

		void GUIActive ()
		{
				if ((CogPull >= SubmitTresholdHigh || Input.GetKeyDown (KeyCode.H)) && !IsGuiActive && CanToggleGUI) { // pull
						IsGuiActive = true;
						CanToggleGUI = false; //only toggle on rising edge
						InputHandler.setGUIMode (IsGuiActive);
				}
		
				if ((CogPull >= SubmitTresholdHigh || Input.GetKeyDown (KeyCode.H)) && IsGuiActive && CanToggleGUI) { // pull
						IsGuiActive = false;
						CanToggleGUI = false; //only toggle on rising edge
						InputHandler.setGUIMode (IsGuiActive);
				}
		
				if (CogPull <= SubmitTresholdLow) {
						CanToggleGUI = true; 
				}
		}

		void Interactable ()
		{
				if (!IsGuiActive && PreIsGuiActive) {
						foreach (Selectable s in Buttons) {
								s.interactable = false;
						}
				}	
				if (IsGuiActive && !PreIsGuiActive) {
						foreach (Selectable s in Buttons) {
								s.interactable = true;
						}
				}	
				PreIsGuiActive = IsGuiActive;
		}

		void CheckInput ()
		{
				v = Input.GetAxisRaw ("Vertical");
				if (CogRight >= SubmitTresholdHigh)
						v = CogRight; 		
				if (CogLeft >= SubmitTresholdHigh)
						v = -CogLeft;	

				//scroll through elements in gui when up/down is high
				if (v != 0f)
						ScrollTimer += Time.deltaTime; //seconds vertical axis has been held 
				if ((v < 0f) && ((PreV == 0f) || (ScrollTimer >= AutoScrollTime))) {
						GoDown ();
						ScrollTimer = 0;
				} else if ((v > 0f) && ((PreV == 0f) || (ScrollTimer >= AutoScrollTime))) {
						GoUp ();
						ScrollTimer = 0;
				}
				PreV = v;

				//map cogPush or "G" to submit/enter
				if (((CogPush >= SubmitTresholdHigh) && CanSubmit) || (Input.GetKeyDown (KeyCode.G))) {
						//simulate "enter" pressed
						ExecuteEvents.Execute<ISubmitHandler> (SelectedButton.gameObject, new BaseEventData (Eventsystem), ExecuteEvents.submitHandler);
						CanSubmit = false;		//only activate on rising edge
				} else if (CogPush <= SubmitTresholdLow) {
						CanSubmit = true;
				}
		}


		//select next element in gui
		void GoDown ()
		{
				if (SelectedButtonIndex >= Buttons.Count - 1) {
						SelectedButtonIndex = 0;
				} else {
						SelectedButtonIndex = SelectedButtonIndex + 1;
				}
		
				SelectedButton = Buttons [SelectedButtonIndex];
				Eventsystem.SetSelectedGameObject (SelectedButton.gameObject, new BaseEventData (Eventsystem));
		}
	
		//select previous element in gui
		void GoUp ()
		{
				if (SelectedButtonIndex <= 0) {
						SelectedButtonIndex = Buttons.Count - 1;
				} else {
						SelectedButtonIndex = SelectedButtonIndex - 1;
				}
		
				SelectedButton = Buttons [SelectedButtonIndex];
				Eventsystem.SetSelectedGameObject (SelectedButton.gameObject, new BaseEventData (Eventsystem));
		}
}