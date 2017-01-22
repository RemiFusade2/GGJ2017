using UnityEngine;
using System.Collections;

public class dropthatbase : MonoBehaviour {

	public GameObject _dropA;
	public GameObject _dropB;
	public GameObject _dropC;
	public GameObject _loopA;
	public GameObject _loopB;
	public GameObject _loopC;
	public GameObject _loopD;

	public bool _gameLaunched;
	public bool _gameStarted;

	public int _musicState;

	// Use this for initialization
	void Start () {
		_musicState = 1;
		_gameStarted = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (_dropA.GetComponent<AudioSource> ().isPlaying == false && _gameStarted && _musicState == 1) {

			// ci-gît "_dropA.SetActive (false);"   :'(
			_loopA.SetActive (true);

			_musicState = 2;

		}

		// pk j'suis obligé de faire ça wesh ?
		if (_loopA.GetComponent<AudioSource> ().isPlaying) {

			_dropA.SetActive (false);

		}

		if (_musicState == 3) {

			_loopA.GetComponent<AudioSource> ().loop = false;

		}

		if (_musicState == 4 && _dropB.GetComponent<AudioSource> ().isPlaying == false) {

			_dropB.SetActive (false);
			_loopB.SetActive (true);
			_musicState = 5;

		}

		if (_musicState == 3 && _loopA.GetComponent<AudioSource> ().isPlaying == false) {

			_loopA.SetActive (false);
			_dropB.SetActive (true);
			_musicState = 4;

		}

		if (_musicState == 7 && _dropC.GetComponent<AudioSource> ().isPlaying == false) {

			_dropC.SetActive (false);
			_loopC.SetActive (true);
			_musicState = 8;

		}

		if (_musicState == 6) {

			_loopB.GetComponent<AudioSource> ().loop = false;

		}

		if (_musicState == 6 && _loopB.GetComponent<AudioSource> ().isPlaying == false) {

			_loopB.SetActive (false);
			_loopA.SetActive (false);
			_dropC.SetActive (true);
			_musicState = 7;

		}


	}

	public void BoostTheMusic ()
	{

		_musicState++;

	}

	public void SetMusicState (int state)
	{

		_musicState = state;

	}

	public AudioSource wastedSound;

	public void StopMusic()
	{
		//StartCoroutine(WaitAndDecreaseVolume(0.05f));
		wastedSound.Play ();

		if (_dropA.activeInHierarchy) {
			_dropA.GetComponent<AudioSource> ().Stop();
		}
		if (_dropB.activeInHierarchy) {
			_dropB.GetComponent<AudioSource> ().Stop();
		}
		if (_dropC.activeInHierarchy) {
			_dropC.GetComponent<AudioSource> ().Stop();
		}
		if (_loopA.activeInHierarchy) {
			_loopA.GetComponent<AudioSource> ().Stop();
		}
		if (_loopB.activeInHierarchy) {
			_loopB.GetComponent<AudioSource> ().Stop();
		}
		if (_loopC.activeInHierarchy) {
			_loopC.GetComponent<AudioSource> ().Stop();
		}
	}

	IEnumerator WaitAndDecreaseVolume(float timer)
	{
		yield return new WaitForSeconds (timer);

		if (_dropA.activeInHierarchy) {
			_dropA.GetComponent<AudioSource> ().volume -= 0.01f;
		}
		if (_dropB.activeInHierarchy) {
			_dropB.GetComponent<AudioSource> ().volume -= 0.01f;
		}
		if (_dropC.activeInHierarchy) {
			_dropC.GetComponent<AudioSource> ().volume -= 0.01f;
		}
		if (_loopA.activeInHierarchy) {
			_loopA.GetComponent<AudioSource> ().volume -= 0.01f;
		}
		if (_loopB.activeInHierarchy) {
			_loopB.GetComponent<AudioSource> ().volume -= 0.01f;
		}
		if (_loopC.activeInHierarchy) {
			_loopC.GetComponent<AudioSource> ().volume -= 0.01f;
		}

		StartCoroutine(WaitAndDecreaseVolume(0.05f));
	}
}
