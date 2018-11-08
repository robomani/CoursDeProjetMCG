using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
#if UNITY_CHEAT
    private bool IsCheating = false;
#endif
    private bool m_GameEnded;
    private bool m_WaitForMulligan = true;
    private Vector3 m_TempPosition;
    private Ray m_RayPlayerHand;
    private Card m_LastCard = null;
    private Card m_SelectedCard = null;

    [SerializeField]
    private GameController m_Game;


    public void StopWaitingForMulligan()
    {
        m_WaitForMulligan = false;
    }

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
                if (m_Game.TurnOwner == Card.Players.Player && !m_WaitForMulligan)
                {
                    Debug.DrawRay(m_RayPlayerHand.origin, m_RayPlayerHand.direction);

                    if (Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Avatar AI")))
                    {
                        if (Input.GetButtonDown("Select") && TileHitInfo.transform.GetComponentInParent<TileController>().m_IsValidTarget && m_SelectedCard != null)
                        {
                            m_Game.SelectedCardAttackAIAvatar(m_SelectedCard);
                        }
                    }
                    else if (Physics.Raycast(m_RayPlayerHand, out HitInfo, 1000f, LayerMask.GetMask("Card")))
                    {
                        Card CardOver = HitInfo.transform.GetComponent<Card>();
                        if (m_LastCard != null && m_LastCard != CardOver && m_LastCard != m_SelectedCard && m_LastCard.m_Owner == m_Game.TurnOwner && m_LastCard.m_Playable)
                        {
                            m_LastCard.UnIlluminate();
                        }

                        if (Input.GetButtonDown("Select") && m_SelectedCard != null && m_SelectedCard.CardType == CardType.Component && CardOver.m_ValidTarget)
                        {
                            m_Game.SelectedComponentAddToCard(CardOver,m_SelectedCard);
                        }
                        else if (Input.GetButtonDown("Select") && (CardOver.State == Card.States.InHand || CardOver.State == Card.States.InPlay) && CardOver.m_Owner == m_Game.TurnOwner && CardOver.m_Playable)
                        {
                            if (m_SelectedCard != CardOver && ((CardOver.State == Card.States.InHand && CardOver.CastingCost <= m_Game.PlayerMana) || (CardOver.State == Card.States.InPlay && CardOver.ActivateCost <= m_Game.PlayerMana)))
                            {
                                if (m_SelectedCard != null)
                                {
                                    m_SelectedCard.UnIlluminate();
                                }
                                m_SelectedCard = CardOver;

                                if (m_SelectedCard.State == Card.States.InHand)
                                {
                                    m_Game.showButtonDiscard(true, m_SelectedCard);
                                }
                                m_Game.ShowNormalTiles(m_SelectedCard);
                                m_SelectedCard.SelectedColor();
                                m_Game.ShowValidTiles(m_SelectedCard);
                            }
                            else
                            {
                                m_Game.ShowNormalCards();
                                if (m_SelectedCard != null)
                                {
                                    m_SelectedCard.UnIlluminate();
                                }
                                m_Game.ShowNormalTiles(m_SelectedCard);
                                m_SelectedCard = null;
                                m_Game.BtnDiscard.SetActive(false);
                            }

                        }
                        else if (Input.GetButtonDown("Select") && CardOver.State == Card.States.InPlay && CardOver.m_ValidTarget)
                        {
                            m_SelectedCard.AttackCard(CardOver);
                            m_SelectedCard = null;
                        }


                        if (Input.GetButtonDown("Zoom") && (CardOver.State == Card.States.InHand || CardOver.State == Card.States.InPlay))
                        {
                            m_Game.Zoom(true, CardOver);
                        }

                        if (m_SelectedCard != CardOver && CardOver != m_Game.ZoomCard.GetComponent<Card>() && CardOver.m_Owner == m_Game.TurnOwner && CardOver.m_Playable)
                        {
                            if (m_SelectedCard == null || m_SelectedCard.CardType != CardType.Component)
                            {
                                CardOver.Illuminate();
                            }
                        }

                        if (m_SelectedCard == null && ((CardOver.State != Card.States.InDeck && CardOver.State != Card.States.InGrave) && ((CardOver.State == Card.States.InPlay && CardOver.m_Owner == Card.Players.AI) || CardOver.m_Owner == Card.Players.Player)))
                        {
                            m_Game.ShowValidTiles(CardOver);
                        }
                        m_LastCard = CardOver;
                    }
                    else
                    {
                        if (m_LastCard != null && m_LastCard != m_SelectedCard && m_LastCard.m_Owner == m_Game.TurnOwner && m_LastCard.m_Playable)
                        {
                            m_LastCard.UnIlluminate();
                            m_LastCard.m_CardName = "";
                        }

                        if (m_SelectedCard == null)
                        {
                            m_Game.ShowNormalTiles(null);
                        }

                        if (Input.GetButtonDown("Zoom"))
                        {
                            m_Game.Zoom(false);
                        }
                    }

                    if (Input.GetButtonDown("Select") && m_SelectedCard != null && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Tiles")) && TileHitInfo.transform.GetComponent<TileController>().m_IsValid)
                    {
                        m_TempPosition = TileHitInfo.transform.position;
                        if (m_SelectedCard.ShadowTime > 0)
                        {
                            TileHitInfo.transform.GetComponent<TileController>().SetShadow(m_SelectedCard.ShadowTime, m_SelectedCard.m_Owner);
                        }
                        m_SelectedCard.StartCoroutine(m_SelectedCard.CardMove(m_TempPosition, m_Game.PlayerTileRotation.rotation));
                        if (m_SelectedCard.State == Card.States.InHand)
                        {
                            if (m_SelectedCard.CardType == CardType.Spell)
                            {
                                m_Game.CastSpell(TileHitInfo.transform.GetComponent<TileController>(), m_SelectedCard);
                            }
                            else
                            {
                                m_Game.SelectedCardToInPlayState(m_SelectedCard);
                            }
                        }
                        else
                        {
                            m_SelectedCard.m_TileOccupied.m_OccupiedBy = null;
                            m_Game.ChangePlayerMana(-m_SelectedCard.ActivateCost);
                        }
                        if (m_SelectedCard)
                        {
                            m_Game.SelectedCardToBoard(TileHitInfo.transform.GetComponent<TileController>(), m_SelectedCard);
                        }
                    }

                    if (Input.GetButtonDown("Select") && Physics.Raycast(m_RayPlayerHand, out TileHitInfo, 1000f, LayerMask.GetMask("Avatar Player")) && m_Game.PlayerMana > 0)
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
                            m_Game.m_IsCheating = false;
                            m_Game.AltarPlayer.GetComponent<Renderer>().material.color = Color.white;
                        }
                        else
                        {
                            IsCheating = true;
                            m_Game.m_IsCheating = true;
                            m_Game.AltarPlayer.GetComponent<Renderer>().material.color = Color.yellow;
                        }
                    }
#endif
                }
                else if (m_Game.TurnOwner == Card.Players.AI && !m_WaitForMulligan)
                {

                }
            }
            if (m_SelectedCard != null)
            {
                m_Game.ShowValidTiles(m_SelectedCard);
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
