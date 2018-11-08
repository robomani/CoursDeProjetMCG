using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private GameController m_Game;

    private bool m_GameEnded;
    private Ray m_RayPlayerHand;

#if UNITY_CHEAT
    private bool IsCheating = false;
#endif

    private void Update()
    {
        if (Input.GetButtonDown("Options"))
        {

        }

        if (!m_GameEnded)
        {
            if (m_Game.PlayerHp <= 0 || m_Game.HpAI <= 0)
            {
                m_Game.GameEnd(m_Game.HpAI > 0 ? false : true);
            }

            if (!m_Game.m_MainCamera.enabled)
            {
                if (Input.GetButtonDown("Zoom"))
                {
                    m_Game.Zoom(false);
                }
            }
            else
            {
                m_RayPlayerHand = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit HitInfo;
                RaycastHit TileHitInfo;
                if (m_Game.TurnOwner == Card.Players.Player && !m_Game.WaitForMulligan)
                {
                    Debug.DrawRay(m_RayPlayerHand.origin, m_RayPlayerHand.direction);

                    if (Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Avatar AI")))
                    {
                        if (Input.GetButtonDown("Select") && TileHitInfo.transform.GetComponentInParent<TileController>().m_IsValidTarget && m_Game.SelectedCard != null)
                        {
                            m_Game.SelectedCardAttackAIAvatar();
                        }
                    }
                    else if (Physics.Raycast(m_RayPlayerHand, out HitInfo, 1000f, LayerMask.GetMask("Card")))
                    {
                        Card CardOver = HitInfo.transform.GetComponent<Card>();
                        if (m_Game.LastCard != null && m_Game.LastCard != CardOver && m_Game.LastCard != m_Game.SelectedCard && m_Game.LastCard.m_Owner == m_Game.TurnOwner && m_Game.LastCard.m_Playable)
                        {
                            m_Game.LastCard.UnIlluminate();
                        }

                        if (Input.GetButtonDown("Select") && m_Game.SelectedCard != null && m_Game.SelectedCard.CardType == CardType.Component && CardOver.m_ValidTarget)
                        {
                            m_Game.SelectedComponentAddToCard(CardOver);
                        }
                        else if (Input.GetButtonDown("Select") && (CardOver.State == Card.States.InHand || CardOver.State == Card.States.InPlay) && CardOver.m_Owner == m_Game.TurnOwner && CardOver.m_Playable)
                        {
                            if (m_Game.SelectedCard != CardOver && ((CardOver.State == Card.States.InHand && CardOver.CastingCost <= m_Game.PlayerMana) || (CardOver.State == Card.States.InPlay && CardOver.ActivateCost <= m_Game.PlayerMana)))
                            {
                                if (m_Game.SelectedCard != null)
                                {
                                    m_Game.SelectedCard.UnIlluminate();
                                }
                                m_Game.SelectedCard = CardOver;

                                if (m_Game.SelectedCard.State == Card.States.InHand)
                                {
                                    m_Game.showButtonDiscard(true);
                                }
                                m_Game.ShowNormalTiles();
                                m_Game.SelectedCard.SelectedColor();
                                m_Game.ShowValidTiles(m_Game.SelectedCard);
                            }
                            else
                            {
                                m_Game.ShowNormalCards();
                                if (m_Game.SelectedCard != null)
                                {
                                    m_Game.SelectedCard.UnIlluminate();
                                }
                                m_Game.ShowNormalTiles();
                                m_Game.SelectedCard = null;
                                m_Game.BtnDiscard.SetActive(false);
                            }

                        }
                        else if (Input.GetButtonDown("Select") && CardOver.State == Card.States.InPlay && CardOver.m_ValidTarget)
                        {
                            m_Game.AttackCard(m_Game.SelectedCard, CardOver);
                        }


                        if (Input.GetButtonDown("Zoom") && (CardOver.State == Card.States.InHand || CardOver.State == Card.States.InPlay))
                        {
                            m_Game.Zoom(true, CardOver);
                        }

                        if (m_Game.SelectedCard != CardOver && CardOver != ZoomCard.GetComponent<Card>() && CardOver.m_Owner == m_Game.TurnOwner && CardOver.m_Playable)
                        {
                            if (m_Game.SelectedCard == null || m_Game.SelectedCard.CardType != CardType.Component)
                            {
                                CardOver.Illuminate();
                            }
                        }

                        if (m_Game.SelectedCard == null && ((CardOver.State != Card.States.InDeck && CardOver.State != Card.States.InGrave) && ((CardOver.State == Card.States.InPlay && CardOver.m_Owner == Card.Players.AI) || CardOver.m_Owner == Card.Players.Player)))
                        {
                            m_Game.ShowValidTiles(CardOver);
                        }
                        m_Game.LastCard = CardOver;
                    }
                    else
                    {
                        if (m_Game.LastCard != null && m_Game.LastCard != m_Game.SelectedCard && m_Game.LastCard.m_Owner == m_Game.TurnOwner && m_Game.LastCard.m_Playable)
                        {
                            m_Game.LastCard.UnIlluminate();
                            m_Game.LastCard.m_CardName = "";
                        }

                        if (m_Game.SelectedCard == null)
                        {
                            m_Game.ShowNormalTiles();
                        }

                        if (Input.GetButtonDown("Zoom"))
                        {
                            m_Game.Zoom(false);
                        }
                    }

                    if (Input.GetButtonDown("Select") && m_Game.SelectedCard != null && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Tiles")) && TileHitInfo.transform.GetComponent<TileController>().m_IsValid)
                    {
                        m_Game.TempPosition = TileHitInfo.transform.position;
                        if (m_Game.SelectedCard.ShadowTime > 0)
                        {
                            TileHitInfo.transform.GetComponent<TileController>().SetShadow(m_Game.SelectedCard.ShadowTime, m_Game.SelectedCard.m_Owner);
                        }
                        m_Game.SelectedCard.StartCoroutine(m_Game.SelectedCard.CardMove(m_Game.TempPosition, m_Game.PlayerTileRotation.rotation));
                        if (m_Game.SelectedCard.State == Card.States.InHand)
                        {
                            if (m_Game.SelectedCard.CardType == CardType.Spell)
                            {
                                m_Game.CastSpell(TileHitInfo.transform.GetComponent<TileController>());
                            }
                            else
                            {
                                m_Game.SelectedCardToInPlayState();
                            }
                        }
                        else
                        {
                            m_Game.SelectedCard.m_TileOccupied.m_OccupiedBy = null;
                            m_Game.ChangePlayerMana(-m_Game.SelectedCard.ActivateCost);
                        }
                        if (m_Game.SelectedCard)
                        {
                            m_Game.SelectedCardToBoard(TileHitInfo.transform.GetComponent<TileController>());
                        }
                    }

                    if (Input.GetButtonDown("Select") && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Avatar Player")) && m_PlayerMana > 0)
                    {
                        m_Game.ActivatePlayerAvatar();
                    }
#if UNITY_CHEAT
                    if (Input.GetButtonDown("Cheat"))
                    {
                        m_Game.DrawCard();
                        m_Game.ChangePlayerMana(10);
                        m_Game.PlayerHp += 1;
                    }
                    if (Input.GetKeyDown(KeyCode.D))
                    {
                        m_Game.DrawCard();
                    }
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        m_Game.ChangePlayerMana(10);
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        m_Game.PlayerHp += 1;
                    }
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (IsCheating)
                        {
                            IsCheating = false;
                            m_Game.AltarPlayer.GetComponent<Renderer>().material.color = Color.white;
                        }
                        else
                        {
                            IsCheating = true;
                            m_Game.AltarPlayer.GetComponent<Renderer>().material.color = Color.yellow;
                        }
                    }
#endif
                }
                else if (m_Game.TurnOwner == Card.Players.AI && !m_Game.WaitForMulligan)
                {

                }
            }
            if (m_Game.SelectedCard != null)
            {
                m_Game.ShowValidTiles(m_Game.SelectedCard);
            }
            m_Game.UpdateTexts();
#if UNITY_CHEAT
            if (IsCheating)
            {
                m_Game.AltarPlayer.GetComponent<Renderer>().material.color = Color.yellow;
            }
#endif
        }
    }

}
