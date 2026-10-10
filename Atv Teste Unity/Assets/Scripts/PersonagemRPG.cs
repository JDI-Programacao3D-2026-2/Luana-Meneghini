using System.Reflection;
using UnityEngine;

public class PersonagemRPG : MonoBehaviour
{
    string nome = "Arthas";
    int vida = 50;
    float velocidade = 7.5f;
    int nivel = 3;
    bool estaVivo = true;
    const int vidaMaxima = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        string FichaDoPersonagem = $"Nome: {nome} | Nível: {nivel} | Vida: {vida}/{vidaMaxima} | Velocidade: {velocidade} | Status: {estaVivo}";
        Debug.Log(FichaDoPersonagem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
