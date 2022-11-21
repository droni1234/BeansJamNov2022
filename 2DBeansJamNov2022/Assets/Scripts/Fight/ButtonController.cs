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

        private Color original;
        private Color highlight;

        // Start is called before the first frame update
        private void Start()
        {
            image = GetComponent<Image>();
            original = image.color;
            highlight = new Color(original.r + 0.5F, original.g + 0.5F, original.b + 0.5F, original.a);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButton(triggerButton))
            {
                image.sprite = pressedSprite;
                image.color = highlight;
            }
            else
            {
                image.color = original;
                image.sprite = defaultSprite;
            }
            //image.sprite = Input.GetButton(triggerButton) ? pressedSprite : defaultSprite;
        }
    }
}