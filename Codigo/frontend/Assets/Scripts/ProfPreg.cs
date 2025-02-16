using System;
using System.Collections.Generic;

[Serializable]
public class Pregunta
{
    public string id; // Identificador de la pregunta en MongoDB
    public string rQuestion;
    public List<rOpcion> rOptions;
    public string rAnswer;
}

[Serializable]
public class rOpcion
{
    public string option;
    public string text;
}