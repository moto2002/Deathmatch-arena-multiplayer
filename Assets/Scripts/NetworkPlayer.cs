﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool>{}

public class NetworkPlayer : NetworkBehaviour {

	[SerializeField] ToggleEvent m_OnToggleShared;
	[SerializeField] ToggleEvent m_OnToggleLocal;
	[SerializeField] ToggleEvent m_OnToggleRemote;

	[SerializeField] float m_RespawnTime = 1f;
	GameObject m_MainCamera;

	// Use this for initialization
	void Start () {
		m_MainCamera = Camera.main.gameObject;
		EnablePlayer();
	}

	void EnablePlayer(){
		m_OnToggleShared.Invoke(true);
		if(isLocalPlayer){
			m_MainCamera.SetActive(false);
			m_OnToggleLocal.Invoke(true);
		}
		else{
			m_OnToggleRemote.Invoke(true);
		}
	}
	
	void DisablePlayer(){
		m_OnToggleShared.Invoke(false);
		if(isLocalPlayer){
			m_MainCamera.SetActive(true);
			m_OnToggleLocal.Invoke(false);
		}
		else{
			m_OnToggleRemote.Invoke(false);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Die(){
		DisablePlayer();
		Invoke("Respawn", m_RespawnTime);
	}

	void Respawn(){
		if(isLocalPlayer){
			Transform spawn = NetworkManager.singleton.GetStartPosition();
			transform.position = spawn.position;
			transform.rotation = spawn.rotation;	
		}
		EnablePlayer();
	}
}
