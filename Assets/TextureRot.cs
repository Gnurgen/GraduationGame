using UnityEngine;
using System.Collections;

public class TextureRot : MonoBehaviour {

    float rotation = 0;
    float scale = 10;
    Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    Vector3 tiling = new Vector3(1, 1, 1);
    Material animMat;

    // Use this for initialization
    void Start()
    {
        animMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        rotation += Time.deltaTime * scale;
        Quaternion quat = Quaternion.Euler(0, 0, rotation);
        Matrix4x4 matrix1 = Matrix4x4.TRS(offset, Quaternion.identity, tiling);
        Matrix4x4 matrix2 = Matrix4x4.TRS(Vector3.zero, quat, tiling);
        Matrix4x4 matrix3 = Matrix4x4.TRS(-offset, Quaternion.identity, tiling);
        animMat.SetMatrix("_Matrix", matrix1 * matrix2 * matrix3);
    }
}
