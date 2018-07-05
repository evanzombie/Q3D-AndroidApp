using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TermsPrivacy : MonoBehaviour {

	public void privacyPage(){
		Application.OpenURL ("http://quantum3d.com/privacy/");
	}
	public void termsPage(){
		Application.OpenURL ("http://quantum3d.com/terms-of-service/");
	}
}
