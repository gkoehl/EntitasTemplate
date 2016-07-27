using UnityEngine;
using Entitas;
using UnityEngine.UI;
using System;

namespace RMC.EntitasTemplate.Entitas.Controllers
{
	/// <summary>
	/// Replace me with description.
	/// </summary>
	public class CanvasController : MonoBehaviour
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
				private Group _gameScore;

		// ------------------ Methods


        public Text _scoreText;
        public Button _restartButton;
		public Button _pauseButton;

		// ------------------ Non-serialized fields
		private Group _scoreGroup;
		// ------------------ Methods

        protected void Start()
        {
			Debug.Log ("CC.Start()");
            _scoreGroup = Pools.pool.GetGroup(Matcher.AllOf (Matcher.Game));
			Debug.Log ("_scoreGroup: " + _scoreGroup.count);

			//TODO: these listeners are not working
            _scoreGroup.OnEntityUpdated += OnScoreUpdated;
			_scoreGroup.OnEntityAdded += OnScoreAdded;

			var scoreComponent = _scoreGroup.GetSingleEntity().score;
			SetScore (scoreComponent.whiteScore, scoreComponent.blackScore);

			_scoreGroup.GetSingleEntity().ReplaceScore (10,10);
			_scoreGroup.GetSingleEntity().ReplaceScore (11,11);

			_restartButton.onClick.AddListener (OnRestartButtonClicked);
			_pauseButton.onClick.AddListener (OnPauseButtonClicked);
        }



        private void SetScore(int whiteScore, int blackScore)
        {
            _scoreText.text = string.Format ("White: {0}    Black: {0}", whiteScore, blackScore);
        }

        protected void OnDestroy()
        {
			_restartButton.onClick.RemoveListener (OnRestartButtonClicked);
			_pauseButton.onClick.RemoveListener (OnPauseButtonClicked);
        }

        private void OnScoreUpdated(Group group, Entity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
            Debug.Log ("score updated");
        }
        private void OnScoreAdded(Group @group, Entity entity, int index, IComponent component)
        {
			Debug.Log ("score change");
			//var scoreComponent = entity.score;
           	//SetScore(scoreComponent.whiteScore, scoreComponent.blackScore);
        }

		private void OnRestartButtonClicked()
        {
           GameController.Instance.Restart();
        }
		private void OnPauseButtonClicked()
        {
           GameController.Instance.TogglePause();
        }

	}
}
