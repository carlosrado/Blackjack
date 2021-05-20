﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    private bool isStanded = false;
    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posición de cada valor se deberá corresponder con la posición de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] debería haber un 2.
         */
        int n = 0;
        //mientras n no sea 52
        while (n != faces.Length)
        {
            //para crear los cuatro palos
            for (int j = 1; j <= 4; j++)
            {
                //para crear las cuatro cartas
                for (int i = 1; i <= 13; i++)
                {
                    //1-10 tienen su numero
                    if (i <= 10)
                    {
                        values[n] = i;
                        n++;
                    }
                    //las figuras valen diez
                    else
                    {
                        values[n] = 10;
                        n++;
                    }
                }
            }
        }
        

    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */
        for(int i = 0;i< values.Length; i++)
        {
            int random = Random.Range(0, 52);
            int aux;
            Sprite auxSprite;
            aux = values[i];
            values[i] = values[random];
            values[random] = aux;
            auxSprite = faces[i];
            faces[i] = faces[random];
            faces[random] = auxSprite;
        }
    }

    void StartGame()
    {
        isStanded = false;
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
            if (dealer.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "Has perdido, vuelve a intentarlo";
            }
            if (player.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "Has ganado, enhorabuena";
            }
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
        int puntosPlayer =player.GetComponent<CardHand>().points;
        int puntosDealer = player.GetComponent<CardHand>().points;


    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        if (isStanded == false)
        {
            //Repartimos carta al jugador
            PushPlayer();

            /*TODO:
             * Comprobamos si el jugador ya ha perdido y mostramos mensaje
             */
            if (player.GetComponent<CardHand>().points > 21)
            {
                finalMessage.text = "Has perdido, vuelve a intentarlo";
            }
        }
       
    }

    public void Stand()
    {
        isStanded = true;
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        if (dealer.GetComponent<CardHand>().cards.Count == 2)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
        }
        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        while (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();
        }
        if(dealer.GetComponent<CardHand>().points < player.GetComponent<CardHand>().points || dealer.GetComponent<CardHand>().points>21)
        {
            finalMessage.text = "Has ganado, enhorabuena";
        }
        else if(dealer.GetComponent<CardHand>().points > player.GetComponent<CardHand>().points)
        {
            finalMessage.text = "Has perdido, vuelve a intentarlo";
        }
        else
        {
            finalMessage.text = "Has empatado, vuelve a intentarlo";

        }
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
