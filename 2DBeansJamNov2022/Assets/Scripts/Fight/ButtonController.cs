using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace whip.battle
{
    public class ButtonController : MonoBehaviour
    {

        private Image image;

        public Sprite defaultSprite;
        public Sprite pressedSprite;

        public string triggerButton;

        // Start is called before the first frame update
        private void Start()
        {
            image = GetComponent<Image>();
        }

        // Update is called once per frame
        private void Update()
        {
            image.sprite = Input.GetButton(triggerButton) ? pressedSprite : defaultSprite;
        }
    }
}