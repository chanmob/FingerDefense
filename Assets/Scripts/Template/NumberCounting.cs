using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberCounting : Singleton<NumberCounting>
{
    public void StartCount(Text _text, float _min, float _max, float _time)
    {
        StartCoroutine(CountingCoroutine(_text, _min, _max, _time));
    }

    private IEnumerator CountingCoroutine(Text _text, float _min, float _max, float _time)
    {
        float offset = (_max - _min) / _time;

        while(_min < _max)
        {
            _min += offset * Time.deltaTime;
            _text.text = "이번 웨이브 : " + ((int)_min).ToString();
            yield return null;
        }

        _min = _max;
        _text.text = "이번 웨이브 : " + ((int)_min).ToString();
    }
}
