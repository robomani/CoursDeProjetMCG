using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    /*
    [SerializeField]
    private GameController m_Game;

    private IEnumerator AICheatTurn()
    {
        yield return new WaitForSeconds(m_DrawTime + 0.3f);
        for (int i = 0; i < m_AIHand.Length; i++)
        {
            if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().CardType == CardType.Creature)
            {
                for (int x = 14; x > 11; x--)
                {
                    if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                    {
                        m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_AIHand[i].GetComponent<Card>();
                        m_AIHand[i].GetComponent<Card>().m_TileOccupied = m_Board.m_Tiles[x].GetComponent<TileController>();
                        StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        m_AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                        m_AIHand[i].GetComponent<Card>().m_Position = -1;
                        m_AIHand[i] = null;

                        yield return new WaitForSeconds(0.5f);
                        break;

                    }
                }

            }
            else if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().CardType == CardType.Building)
            {
                for (int x = 9; x < 15; x++)
                {
                    if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                    {
                        m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_AIHand[i].GetComponent<Card>();
                        m_AIHand[i].GetComponent<Card>().m_TileOccupied = m_Board.m_Tiles[x].GetComponent<TileController>();
                        StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        m_AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                        m_AIHand[i].GetComponent<Card>().m_Position = -1;
                        m_AIHand[i] = null;

                        yield return new WaitForSeconds(0.5f);
                        break;
                    }
                }
            }
            else if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().CardType == CardType.Component)
            {
                for (int x = 0; x < 15; x++)
                {
                    if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI)
                    {
                        if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType != CardType.Building || !m_AIHand[i].GetComponent<Attack>())
                        {
                            StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                            yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                            m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.AddComponentCard(m_AIHand[i].GetComponent<Card>());
                            Destroy(m_AIHand[i]);
                            m_AIHand[i] = null;
                            yield return new WaitForSeconds(0.5f);
                            break;
                        }

                    }
                }
            }
        }

        for (int x = 0; x < 15; x++)
        {
            if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
            {
                //Attack une colonne plus proche du joueur si possible
                if (m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));           
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack l'avatar du joueur si possible
                else if (x - 3 < 0)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    TileController PlayerAvatar = m_AltarPlayer;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerAvatar.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerAvatar.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    HurtPlayer(AiCard.Attack);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack une colonne plus loin du joueur si possible
                else if (m_Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[Mathf.Max(x + 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    // return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack une ligne plus haut si possible
                else if (x % 3 != 0 && m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Attack une ligne plus bas si possible
                else if ((x + 1) % 3 != 0 && m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    Card PlayerCard = m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy;

                    //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    AttackCard(AiCard, PlayerCard);
                    yield return new WaitForSeconds(0.5f);
                    break;
                }

                //Bouge vers le joueur si possible
                else if (x - 3 >= 0 && m_Board.m_Tiles[x - 3].GetComponent<TileController>().m_OccupiedBy == null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
                {
                    Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                    StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                    m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy = AiCard;
                    m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = null;
                    AiCard.m_TileOccupied = m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>();
                    AiCard = null;
                    yield return new WaitForSeconds(0.5f);
                    break;
                }


            }
        }
        m_TurnAI = false;
        EndTurn();
    }

    private IEnumerator AiTurn()
    {
        yield return new WaitForSeconds(m_DrawTime + 0.3f);
        if (m_AIMaxMana < m_PlayerMaxMana)
        {
            m_AIMaxMana++;
            m_AIMana = 0;
        }

        int turnTry = 0;

        while (m_AIMana > 0 && turnTry <= 10)
        {
            if (GameManager.Instance.m_AICreaturesInPlay < 3)
            {
                bool endLoop = false;
                while (GameManager.Instance.m_AICreaturesInPlay == 0 && !endLoop)
                {
                    for (int i = 0; i < m_AIHand.Length; i++)
                    {
                        if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().CardType == CardType.Creature && m_AIHand[i].GetComponent<Card>().CastingCost <= m_AIMana)
                        {
                            for (int x = 14; x > 11; x--)
                            {
                                if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                                {
                                    m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_AIHand[i].GetComponent<Card>();
                                    m_AIHand[i].GetComponent<Card>().m_TileOccupied = m_Board.m_Tiles[x].GetComponent<TileController>();
                                    GameManager.Instance.AddOrRemoveCreatureToAI(true);
                                    m_AIMana -= m_AIHand[i].GetComponent<Card>().CastingCost;
                                    StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                                    m_AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                                    m_AIHand[i].GetComponent<Card>().m_Position = -1;
                                    m_AIHand[i] = null;
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

            if (m_AIMana <= 0)
            {
                break;
            }

            if (GameManager.Instance.m_AICreaturesInPlay < 2 && GameManager.Instance.m_AIBuildingsInPlay <= 2)
            {
                bool endLoop = false;
                while (GameManager.Instance.m_AICreaturesInPlay == 0 && !endLoop)
                {
                    for (int i = 0; i < m_AIHand.Length; i++)
                    {
                        if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().CardType == CardType.Building && m_AIHand[i].GetComponent<Card>().CastingCost <= m_AIMana)
                        {
                            for (int x = 9; x < 15; x++)
                            {
                                if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy == null)
                                {
                                    m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = m_AIHand[i].GetComponent<Card>();
                                    m_AIHand[i].GetComponent<Card>().m_TileOccupied = m_Board.m_Tiles[x].GetComponent<TileController>();
                                    GameManager.Instance.AddOrRemoveBuildingToAI(true);
                                    m_AIMana -= m_AIHand[i].GetComponent<Card>().CastingCost;
                                    StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                                    m_AIHand[i].GetComponent<Card>().ChangeState(Card.States.InPlay);
                                    m_AIHand[i].GetComponent<Card>().m_Position = -1;
                                    m_AIHand[i] = null;
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

            if (m_AIMana <= 0)
            {
                break;
            }

            if (GameManager.Instance.m_AICreaturesInPlay < 1 && GameManager.Instance.m_AIBuildingsInPlay < 1)
            {
                bool endLoop = false;
                while (m_AIInPlay.Count == 0 && !endLoop)
                {
                    for (int i = 0; i < m_AIHand.Length; i++)
                    {
                        if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().CardType == CardType.Creature)
                        {
                            if (i > 0)
                            {
                                for (int x = i - 1; x >= 0; x--)
                                {
                                    if (m_AIHand[i] != null)
                                    {
                                        AIDiscardCard(m_AIHand[i].GetComponent<Card>());
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

            if (m_AIMana <= 0)
            {
                break;
            }

            if (GameManager.Instance.m_AICreaturesInPlay > 0 || GameManager.Instance.m_AIBuildingsInPlay > 0)
            {
                bool endLoop = false;
                for (int i = 0; i < m_AIHand.Length; i++)
                {
                    if (m_AIHand[i] != null && m_AIHand[i].GetComponent<Card>().CardType == CardType.Component)
                    {
                        for (int x = 0; x < 15; x++)
                        {
                            if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI)
                            {
                                if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType != CardType.Building || !m_AIHand[i].GetComponent<Attack>())
                                {
                                    m_AIMana -= m_AIHand[i].GetComponent<Card>().CastingCost;
                                    StartCoroutine(CardMove(m_AIHand[i].transform, m_AIHand[i].transform.position, m_Board.m_Tiles[x].transform.position, m_AIHand[i].transform.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                                    yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                                    m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.AddComponentCard(m_AIHand[i].GetComponent<Card>());
                                    Destroy(m_AIHand[i]);
                                    m_AIHand[i] = null;
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

            if (m_AIMana <= 0)
            {
                break;
            }

            for (int x = 0; x < 15; x++)
            {
                if (m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.AI && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
                {
                    //Attack une colonne plus proche du joueur si possible
                    if (m_AIMana >= m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                        //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));           
                        //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        AttackCard(AiCard, PlayerCard);
                        yield return new WaitForSeconds(0.5f);
                        m_AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack l'avatar du joueur si possible
                    else if (m_AIMana >= m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && x - 3 < 0)
                    {
                        Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        TileController PlayerAvatar = m_AltarPlayer;

                        //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerAvatar.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        StartCoroutine(CardMove(AiCard.transform, PlayerAvatar.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        HurtPlayer(AiCard.Attack);
                        yield return new WaitForSeconds(0.5f);
                        m_AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack une colonne plus loin du joueur si possible
                    else if (m_AIMana >= m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && m_Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[Mathf.Min(x + 3, 14)].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Board.m_Tiles[Mathf.Max(x + 3, 0)].GetComponent<TileController>().m_OccupiedBy;

                        //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        // return new WaitForSeconds(m_DiscardTime + 0.2f);
                        StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        AttackCard(AiCard, PlayerCard);
                        yield return new WaitForSeconds(0.5f);
                        m_AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack une ligne plus haut si possible
                    else if (m_AIMana >= m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && x % 3 != 0 && m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Board.m_Tiles[x - 1].GetComponent<TileController>().m_OccupiedBy;

                        //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        AttackCard(AiCard, PlayerCard);
                        yield return new WaitForSeconds(0.5f);
                        m_AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Attack une ligne plus bas si possible
                    else if (m_AIMana >= m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && (x + 1) % 3 != 0 && m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy != null && m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy.m_Owner == Card.Players.Player)
                    {
                        Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        Card PlayerCard = m_Board.m_Tiles[x + 1].GetComponent<TileController>().m_OccupiedBy;

                        //StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, PlayerCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        //yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        StartCoroutine(CardMove(AiCard.transform, PlayerCard.transform.position, AiCard.transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        AttackCard(AiCard, PlayerCard);
                        yield return new WaitForSeconds(0.5f);
                        m_AIMana -= AiCard.ActivateCost;
                        break;
                    }

                    //Bouge vers le joueur si possible
                    else if (m_AIMana >= m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.ActivateCost && x - 3 >= 0 && m_Board.m_Tiles[x - 3].GetComponent<TileController>().m_OccupiedBy == null && m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy.CardType == CardType.Creature)
                    {
                        Card AiCard = m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy;
                        StartCoroutine(CardMove(AiCard.transform, AiCard.transform.position, m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().transform.position, m_PlayerTileRotation.rotation, m_PlayerTileRotation.rotation, m_DiscardTime));
                        yield return new WaitForSeconds(m_DiscardTime + 0.2f);
                        m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>().m_OccupiedBy = AiCard;
                        m_Board.m_Tiles[x].GetComponent<TileController>().m_OccupiedBy = null;
                        AiCard.m_TileOccupied = m_Board.m_Tiles[Mathf.Max(x - 3, 0)].GetComponent<TileController>();
                        m_AIMana -= AiCard.ActivateCost;
                        AiCard = null;
                        yield return new WaitForSeconds(0.5f);
                        break;
                    }


                }
            }
            turnTry++;
        }
        m_TurnAI = false;
        EndTurn();
    }
    */
}
