﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Koboct.Data
{
    [Serializable]
    public class Characteristique
    {
        [SerializeField] private TypeCharacteristique _monType;
        [Range(0, 21)] [SerializeField] private int _valeur;
        [Range(-4, 5)] [SerializeField] private int _modificateur;
        public UnityEvent<int> OnValeurChange = new();

        public TypeCharacteristique MonType
        {
            get => _monType;
            set => _monType = value;
        }

        public int Valeur
        {
            set
            {
                if (_valeur == value) return;
                _valeur = value;
                _modificateur = CalculModificateur(_valeur);
                OnValeurChange.Invoke(_valeur);
            }

            get => _valeur;
        }

        public int Modificateur
        {
            get
            {
                _modificateur = CalculModificateur(_valeur);
                return _modificateur;
            }
        }

        public static int CalculModificateur(int val)
        {
            switch (val)
            {
                case 1:
                case 2:
                case 3:
                    return -4;
                case 4:
                case 5:
                    return -3;
                case 6:
                case 7:
                    return -2;
                case 8:
                case 9:
                    return -1;
                case 10:
                case 11:
                    return 0;
                case 12:
                case 13:
                    return 1;
                case 14:
                case 15:
                    return 2;
                case 16:
                case 17:
                    return 3;
                case 18:
                case 19:
                    return 4;
                case 20:
                case 21:
                    return 5;
            }

            return 0;
        }
    }
}