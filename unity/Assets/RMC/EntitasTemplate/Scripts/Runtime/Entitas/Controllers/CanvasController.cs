using UnityEngine;
using Entitas;
using UnityEngine.UI;
using System;
using RMC.EntitasTemplate.Entitas.Controllers.Singleton;

namespace RMC.EntitasTemplate.Entitas.Controllers
{
	/// <summary>
	/// Bridges the Unity GUI and the Entitas
	/// </summary>
	public class CanvasController : MonoBehaviour
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _instructionsText;

		// ------------------ Non-serialized fields
        private Entity _gameEntity;
        private Group _gameScore;

		// ------------------ Methods

        protected void Start()
        {

            Group group = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Score));
            _instructionsText.text = "Use the 4 arrow keys to move. Space to restart.";

            SetGameGroup(group);

        }


        protected void OnDestroy()
        {

        }

        private void SetGameGroup (Group group)
        {
            //The group should have 1 thing, always, but...
            //FIXME: after multiple restarts (via 'r' button in HUD) this fails - srivello
            if (group.count == 1) 
            {
                _gameEntity = group.GetSingleEntity();
                _gameEntity.OnComponentReplaced += Entity_OnComponentReplaced;

                //set first value
                var scoreComponent = _gameEntity.score;
                SetText (scoreComponent.score);

            }
            else
            {
                Debug.LogWarning ("CC _scoreGroup failed, should have one entity, has count: " + group.count);    
            }
        }

        private void Entity_OnComponentReplaced(Entity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
            SetText(entity.score.score);
        }

        private void SetText(int score)
        {
            _scoreText.text = string.Format ("Score: {0}", score);
        }


	}
}
