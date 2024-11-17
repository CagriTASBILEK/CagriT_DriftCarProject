using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
    public class AudioLibrarySO : ScriptableObject
    {
        [Header("Game Sounds")]
        public AudioEventSO gameOver;
        public AudioEventSO gameStart;
    }
}