using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] private float startDelay;
    [SerializeField] private AttackSpot[] attackSpots;
    [SerializeField] private Interaction[] interactions;

    private Image characterSprite;
    private TextMeshProUGUI characterText;

    private float initialSpriteY;

    private void Awake()
    {
        characterSprite = transform.GetChild(0).GetComponent<Image>();
        characterText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        StartCoroutine(DialogCoroutine());

        initialSpriteY = characterSprite.rectTransform.anchoredPosition.y;

        foreach(Interaction interaction in interactions)
        {
            foreach(Attack attack in interaction.attackSeries.attacks)
            {
                attack.attackSpots = attackSpots;
            }
        }
    }

    private void Update()
    {
        characterSprite.rectTransform.anchoredPosition = new Vector2(characterSprite.rectTransform.anchoredPosition.x, initialSpriteY + Mathf.Sin(Time.time) * 20f);
    }

    private IEnumerator DialogCoroutine()
    {
        yield return new WaitForSeconds(startDelay);

        foreach(Interaction interaction in interactions)
        {
            if (interaction.dialog.phrases.Length > 0 & interaction.attackSeries.attacks.Length < 1)
            {
                foreach (string phrase in interaction.dialog.phrases)
                {
                    characterText.text = "> ";

                    for (int i = 0; i < phrase.ToCharArray().Length; i++)
                    {
                        characterText.text += phrase.ToCharArray()[i];

                        AudioManager.Instance.PlaySound("Text" + Random.Range(1, 2));

                        yield return new WaitForSeconds(0.05f);
                    }

                    if (phrase != interaction.dialog.phrases[interaction.dialog.phrases.Length - 1])
                        yield return new WaitForSeconds(interaction.dialog.phraseDelay);
                }

                yield return new WaitForSeconds(interaction.dialog.skipDelay);
            }
            else if (interaction.dialog.phrases.Length < 1 & interaction.attackSeries.attacks.Length > 0)
            {
                foreach (Attack attack in interaction.attackSeries.attacks)
                {
                    yield return new WaitForSeconds(attack.delay);

                    attack.Commit();
                }

                yield return new WaitForSeconds(interaction.attackSeries.skipDelay);
            }
            else if (interaction.dialog.phrases.Length > 0 & interaction.attackSeries.attacks.Length > 0)
            {
                foreach (string phrase in interaction.dialog.phrases)
                {
                    characterText.text = "> ";

                    for (int i = 0; i < phrase.ToCharArray().Length; i++)
                    {
                        characterText.text += phrase.ToCharArray()[i];

                        AudioManager.Instance.PlaySound("Text" + Random.Range(1, 2));

                        yield return new WaitForSeconds(0.05f);
                    }

                    if(phrase != interaction.dialog.phrases[interaction.dialog.phrases.Length - 1])
                        yield return new WaitForSeconds(interaction.dialog.phraseDelay);
                }

                yield return new WaitForSeconds(interaction.dialog.skipDelay);

                foreach (Attack attack in interaction.attackSeries.attacks)
                {
                    yield return new WaitForSeconds(attack.delay);

                    attack.Commit();

                    Player player = FindObjectOfType<Player>();
                    if (player != null)
                    {
                        if(player.canMove)
                            AudioManager.Instance.PlaySound("Attack");
                    }
                }

                yield return new WaitForSeconds(interaction.attackSeries.skipDelay);
            }
        }

        FindObjectOfType<UIManager>().ShowVictoryScreen();

        if (SceneManager.GetActiveScene().buildIndex + 1 < 7)
            SerializeManager.Instance.SetLevelLockedState(SceneManager.GetActiveScene().buildIndex + 1, false);
    }
}
