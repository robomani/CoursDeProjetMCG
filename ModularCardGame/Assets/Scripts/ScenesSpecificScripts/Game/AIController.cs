using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField]
    private GameController m_Game;

    public void AICheatTurn()
    {
        StartCoroutine(AICheat());
    }

    public void AITurn()
    {
        StartCoroutine(AiNormalTurn());
    }

    private void EndTurn()
    {
        m_Game.EndTurn();
    }

    private IEnumerator SelectedCardAttack(Card i_AiCard, Vector3 i_Target, Card i_PlayerCard = null)
    {
        i_AiCard.MoveAttack(i_Target, m_Game.PlayerTileRotation.rotation);
        yield return new WaitForSeconds(m_Game.DiscardTime + 0.2f);  

        if (!i_PlayerCard)
        {
            m_Game.HurtPlayer(i_AiCard.Attack);
        }
        else
        {
            i_AiCard.AttackCard(i_PlayerCard);
        }
        
        yield return new WaitForSeconds(0.5f);
        m_Game.AIMana -= i_AiCard.ActivateCost;
    }

    private IEnumerator AICheat()
    {
        yield return new WaitForSeconds(m_Game.DrawTime + 0.3f);
        for (int i = 0; i < m_Game.AIHand.Length; i++)
        {
            if (m_Game.AIHand[i] != null && m_Game.AIHand[i].GetComponent<Card>().CardType == CardType.Creature)
            {
                for (int x = 14; x > 11; x--)
                {
                    if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                    {
                        m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_Game.AIHand[i].GetComponent<Card>();
                        m_Game.AIHand[i].GetComponent<Card>().m_TileOccupied = m_Game.Board.m_Tiles[x].GetComponent<TileController>();
                        m_Game.AIHand[i].GetComponent<Card>().MoveCard(m_Game.Board.m_Tiles[x].transform.position, m_Game.PlayerTileRotation.rotation);
                        m_Game.AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                        m_Game.AIHand[i].GetComponent<Card>().m_Position = -1;
                        m_Game.AIHand[i] = null;

                        yield return new WaitForSeconds(0.5f);
                        break;

                    }
                }

            }
            else if (m_Game.AIHand[i] != null && m_Game.AIHand[i].GetComponent<Card>().CardType == CardType.Building)
            {
                for (int x = 9; x < 15; x++)
                {
                    if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                    {
                        m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_Game.AIHand[i].GetComponent<Card>();
                        m_Game.AIHand[i].GetComponent<Card>().m_TileOccupied = m_Game.Board.m_Tiles[x].GetComponent<TileController>();
                        m_Game.AIHand[i].GetComponent<Card>().MoveCard(m_Game.Board.m_Tiles[x].transform.position, m_Game.PlayerTileRotation.rotation);
                        m_Game.AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                        m_Game.AIHand[i].GetComponent<Card>().m_Position = -1;
                        m_Game.AIHand[i] = null;

                        yield return new WaitForSeconds(0.5f);
                        break;
                    }
                }
            }
            else if (m_Game.AIHand[i] != null && m_Game.AIHand[i].GetComponent<Card>().CardType == CardType.Component)
            {
                for (int x = 0; x < 15; x++)
                {
                    if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI)
                    {
                        if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType != CardType.Building || !m_Game.AIHand[i].GetComponent<Attack>())
                        {
                            m_Game.AIHand[i].GetComponent<Card>().MoveCard(m_Game.Board.m_Tiles[x].transform.position, m_Game.PlayerTileRotation.rotation);
                            yield return new WaitForSeconds(m_Game.DiscardTime + 0.2f);
                            m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.AddComponentCard(m_Game.AIHand[i].GetComponent<Card>());
                            Destroy(m_Game.AIHand[i]);
                            m_Game.AIHand[i] = null;
                            yield return new WaitForSeconds(0.5f);
                            break;
                        }

                    }
                }
            }
        }

        for (int x = 0; x < 15; x++)
        {
            if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
            {
                //Attack une colonne plus proche du joueur si possible
                if (m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                    StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                    break;
                }

                //Attack l'avatar du joueur si possible
                else if (x - 3 < 0)
                {
                    Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    TileController PlayerAvatar = m_Game.AltarPlayer;

                    StartCoroutine(SelectedCardAttack(AiCard, PlayerAvatar.transform.position));
                    break;
                }

                //Attack une colonne plus loin du joueur si possible
                else if (m_Game.Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Game.Board.m_Tiles[Mathf.Max(x + 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                    StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                    break;
                }

                //Attack une ligne plus haut si possible
                else if (x % 3 != 0 && m_Game.Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Game.Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy;

                    StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                    break;
                }

                //Attack une ligne plus bas si possible
                else if ((x + 1) % 3 != 0 && m_Game.Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Game.Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy;

                    StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                    break;
                }

                //Bouge vers le joueur si possible
                else if (x - 3 >= 0 && m_Game.Board.m_Tiles[x - 3].GetComponent<TileController>().m_OccupiedBy == null && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
                {
                    Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    AiCard.MoveCard(m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().transform.position, m_Game.PlayerTileRotation.rotation);
                    yield return new WaitForSeconds(m_Game.DiscardTime + 0.2f);
                    m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy = AiCard;
                    m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = null;
                    AiCard.m_TileOccupied = m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>();
                    AiCard = null;
                    yield return new WaitForSeconds(0.5f);
                    break;
                }


            }
        }
        //m_Game.TurnAI = false;
        EndTurn();
    }

    private IEnumerator AiNormalTurn()
    {
        yield return new WaitForSeconds(m_Game.DrawTime + 0.3f);
        if (m_Game.AIMaxMana < m_Game.PlayerMaxMana)
        {
            m_Game.AIMaxMana++;
            m_Game.AIMana = 0;
        }

        int turnTry = 0;

        while (m_Game.AIMana > 0 && turnTry <= 10)
        {
            if (GameManager.Instance.m_AICreaturesInPlay < 3)
            {
                bool endLoop = false;
                while (GameManager.Instance.m_AICreaturesInPlay == 0 && !endLoop)
                {
                    for (int i = 0; i < m_Game.AIHand.Length; i++)
                    {
                        if (m_Game.AIHand[i] != null && m_Game.AIHand[i].GetComponent<Card>().CardType == CardType.Creature && m_Game.AIHand[i].GetComponent<Card>().CastingCost <= m_Game.AIMana)
                        {
                            for (int x = 14; x > 11; x--)
                            {
                                if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                                {
                                    m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_Game.AIHand[i].GetComponent<Card>();
                                    m_Game.AIHand[i].GetComponent<Card>().m_TileOccupied = m_Game.Board.m_Tiles[x].GetComponent<TileController>();
                                    GameManager.Instance.AddOrRemoveCreatureToAI(true);
                                    m_Game.AIMana -= m_Game.AIHand[i].GetComponent<Card>().CastingCost;
                                    m_Game.AIHand[i].GetComponent<Card>().MoveCard(m_Game.Board.m_Tiles[x].transform.position, m_Game.PlayerTileRotation.rotation);
                                    m_Game.AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                                    m_Game.AIHand[i].GetComponent<Card>().m_Position = -1;
                                    m_Game.AIHand[i] = null;
                                    endLoop = true;
                                    yield return new WaitForSeconds(0.5f);
                                    break;
                                }
                            }
                            if (endLoop)
                            {
                                break;
                            }
                        }

                    }
                    endLoop = true;
                }
            }

            if (m_Game.AIMana <= 0)
            {
                break;
            }

            if (GameManager.Instance.m_AICreaturesInPlay < 2 && GameManager.Instance.m_AIBuildingsInPlay <= 2)
            {
                bool endLoop = false;
                while (GameManager.Instance.m_AICreaturesInPlay == 0 && !endLoop)
                {
                    for (int i = 0; i < m_Game.AIHand.Length; i++)
                    {
                        if (m_Game.AIHand[i] != null && m_Game.AIHand[i].GetComponent<Card>().CardType == CardType.Building && m_Game.AIHand[i].GetComponent<Card>().CastingCost <= m_Game.AIMana)
                        {
                            for (int x = 9; x < 15; x++)
                            {
                                if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                                {
                                    m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_Game.AIHand[i].GetComponent<Card>();
                                    m_Game.AIHand[i].GetComponent<Card>().m_TileOccupied = m_Game.Board.m_Tiles[x].GetComponent<TileController>();
                                    GameManager.Instance.AddOrRemoveBuildingToAI(true);
                                    m_Game.AIMana -= m_Game.AIHand[i].GetComponent<Card>().CastingCost;
                                    m_Game.AIHand[i].GetComponent<Card>().MoveCard(m_Game.Board.m_Tiles[x].transform.position, m_Game.PlayerTileRotation.rotation);
                                    m_Game.AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                                    m_Game.AIHand[i].GetComponent<Card>().m_Position = -1;
                                    m_Game.AIHand[i] = null;
                                    endLoop = true;
                                    yield return new WaitForSeconds(0.5f);
                                    break;

                                }
                            }

                            if (endLoop)
                            {
                                break;
                            }
                        }
                    }
                    endLoop = true;
                }
            }

            if (m_Game.AIMana <= 0)
            {
                break;
            }

            if (GameManager.Instance.m_AICreaturesInPlay < 1 && GameManager.Instance.m_AIBuildingsInPlay < 1)
            {
                bool endLoop = false;
                while (!endLoop)
                {
                    for (int i = 0; i < m_Game.AIHand.Length; i++)
                    {
                        if (m_Game.AIHand[i] != null && m_Game.AIHand[i].GetComponent<Card>().CardType == CardType.Creature)
                        {
                            if (i > 0)
                            {
                                for (int x = i - 1; x >= 0; x--)
                                {
                                    if (m_Game.AIHand[i] != null)
                                    {
                                        m_Game.AIDiscardCard(m_Game.AIHand[i].GetComponent<Card>());
                                        endLoop = true;
                                        break;
                                    }
                                }
                            }

                            if (endLoop)
                            {
                                break;
                            }
                        }
                    }
                    endLoop = true;
                }
            }

            if (m_Game.AIMana <= 0)
            {
                break;
            }

            if (GameManager.Instance.m_AICreaturesInPlay > 0 || GameManager.Instance.m_AIBuildingsInPlay > 0)
            {
                bool endLoop = false;
                for (int i = 0; i < m_Game.AIHand.Length; i++)
                {
                    if (m_Game.AIHand[i] != null && m_Game.AIHand[i].GetComponent<Card>().CardType == CardType.Component)
                    {
                        for (int x = 0; x < 15; x++)
                        {
                            if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost + m_Game.AIHand[i].GetComponent<Card>().ActivateCost + m_Game.AIHand[i].GetComponent<Card>().CastingCost <= m_Game.AIMana)
                            {
                                if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType != CardType.Building || !m_Game.AIHand[i].GetComponent<Attack>())
                                {
                                    m_Game.AIMana -= m_Game.AIHand[i].GetComponent<Card>().CastingCost;
                                    yield return new WaitForSeconds(m_Game.DiscardTime + 0.2f);
                                    m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.AddComponentCard(m_Game.AIHand[i].GetComponent<Card>());
                                    Destroy(m_Game.AIHand[i]);
                                    m_Game.AIHand[i] = null;
                                    yield return new WaitForSeconds(0.5f);
                                    endLoop = true;
                                    break;
                                }
                            }
                        }

                        if (endLoop)
                        {
                            break;
                        }
                    }
                }
            }

            if (m_Game.AIMana <= 0)
            {
                break;
            }

            for (int x = 0; x < 15; x++)
            {
                if (m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
                {
                    //Attack une colonne plus proche du joueur si possible
                    if (m_Game.AIMana >= m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                        StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                        m_Game.AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack l'avatar du joueur si possible
                    else if (m_Game.AIMana >= m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && x - 3 < 0)
                    {
                        Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        TileController PlayerAvatar = m_Game.AltarPlayer;

                        StartCoroutine(SelectedCardAttack(AiCard, PlayerAvatar.transform.position));
                        m_Game.AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack une colonne plus loin du joueur si possible
                    else if (m_Game.AIMana >= m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && m_Game.Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Game.Board.m_Tiles[Mathf.Max(x + 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                        StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                        m_Game.AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack une ligne plus haut si possible
                    else if (m_Game.AIMana >= m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && x % 3 != 0 && m_Game.Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Game.Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy;

                        StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                        m_Game.AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack une ligne plus bas si possible
                    else if (m_Game.AIMana >= m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && (x + 1) % 3 != 0 && m_Game.Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy != null && m_Game.Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Game.Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy;

                        StartCoroutine(SelectedCardAttack(AiCard, PlayerCard.transform.position, PlayerCard));
                        m_Game.AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Bouge vers le joueur si possible
                    else if (m_Game.AIMana >= m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && x - 3 >= 0 && m_Game.Board.m_Tiles[x - 3].GetComponent<TileController>().m_OccupiedBy == null && m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
                    {
                        Card AiCard = m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        AiCard.MoveCard(m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().transform.position, m_Game.PlayerTileRotation.rotation);
                        yield return new WaitForSeconds(m_Game.DiscardTime + 0.2f);
                        m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy = AiCard;
                        m_Game.Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = null;
                        AiCard.m_TileOccupied = m_Game.Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>();
                        m_Game.AIMana -= AiCard.ActivateCost;
                        AiCard = null;
                        yield return new WaitForSeconds(0.5f);
                        break;
                    }


                }
            }
            turnTry++;
        }
        EndTurn();
    }   
 
}
