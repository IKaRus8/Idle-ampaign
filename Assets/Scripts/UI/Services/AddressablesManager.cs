using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressablesManager
{
    /*
    ����� ���������� ���� ��� � ������ ���������,
    ����� �� ��� ������ ������ � �������� � ����������
    ��� �� ������� ����� ����������� ��������� � � ��� ������� �����
    */
   public static void InitAddressables()
    {
        Addressables.InitializeAsync();
    }
}
