using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chair : MonoBehaviour
{
    public GameObject[] chair_parts;
    public GameObject[] next_chir_parts;

    public GameObject chair_attach_timer_obj;
    public Image chair_attach_timer_img;
    public Text chair_attach_timer_txt;

    int part_num;
    public int count_num;

    public bool is_touch;
    public bool is_drilled;

    chair_part _chair_part;
    chuck_roller _chuck_roller;

    public void chair_part_active(bool value)
    {
        if (count_num < chair_parts.Length)
        {
            next_chir_parts[count_num].SetActive(value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("chair_part")&& is_touch)
        {
            _chair_part = other.GetComponent<chair_part>();
            part_num = _chair_part.part_num;
            if (part_num == count_num && !_chair_part.is_attach)
            {
                chair_parts[part_num].SetActive(true);
                next_chir_parts[part_num].SetActive(false);
                if (count_num < chair_parts.Length) { count_num++; } else { return; }
                StartCoroutine(coro_count_timer());
            }
        }
        if(other.CompareTag("chuck") && is_touch)
        {
            _chuck_roller =other.GetComponent<chuck_roller>();
            is_drilled = _chuck_roller.is_rolling;
            if (is_drilled)
            {
                if(count_num < chair_parts.Length) { next_chir_parts[count_num].SetActive(true); }
                _chair_part.is_attach = false;
                chair_attach_timer_obj.SetActive(false);
                chair_attach_timer_img.fillAmount = 0;
            }
        }
    }
    public void is_touch_bool(bool value)
    {
        is_touch = value;
    }

    IEnumerator coro_count_timer()
    {
        chair_attach_timer_obj.SetActive(true);

        for (int i = 5; i >= 0; i--)
        {
            chair_attach_timer_txt.text = i.ToString();
            for (int j = 0; j < 10; j++)
            {
                chair_attach_timer_img.fillAmount += 0.1f;
                yield return yeild_controller.WaitForSeconds(0.1f);
                if (is_drilled) { break; }
            }
            chair_attach_timer_img.fillAmount = 0;
            if (is_drilled) { break; }
        }
        if (!is_drilled)
        {
            chair_attach_timer_obj.SetActive(false);
            chair_parts[part_num].SetActive(false);
            next_chir_parts[part_num].SetActive(true);
            _chair_part.is_attach = true;
            _chair_part.gameObject.SetActive(true);
            _chair_part.gameObject.transform.position = chair_parts[part_num].transform.position;
            count_num--;
            yield return yeild_controller.WaitForSeconds(0.5f);
            _chair_part.is_attach = false;
        }
        else
        {
            yield return yeild_controller.WaitForSeconds(0.5f);
            is_drilled = false;
        }
        
    }
}
