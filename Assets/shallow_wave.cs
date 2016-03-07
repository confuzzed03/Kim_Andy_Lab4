using UnityEngine;
using System.Collections;

public class shallow_wave : MonoBehaviour {
	int size;
    float[,] old_h, h, new_h;


	// Use this for initialization
	void Start () {
		size = 64;

        // Define floating-point 2D arrays
        old_h = new float[size, size];
        h = new float[size, size];
        new_h = new float[size, size];

        //Resize the mesh into a size*size grid
        Mesh mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		Vector3[] vertices=new Vector3[size*size];
		for (int i=0; i<size; i++)
		for (int j=0; j<size; j++) 
		{
			vertices[i*size+j].x=i*0.2f-size*0.1f;
			vertices[i*size+j].y=0;
			vertices[i*size+j].z=j*0.2f-size*0.1f;
		}
		int[] triangles = new int[(size - 1) * (size - 1) * 6];
		int index = 0;
		for (int i=0; i<size-1; i++)
		for (int j=0; j<size-1; j++)
		{
			triangles[index*6+0]=(i+0)*size+(j+0);
			triangles[index*6+1]=(i+0)*size+(j+1);
			triangles[index*6+2]=(i+1)*size+(j+1);
			triangles[index*6+3]=(i+0)*size+(j+0);
			triangles[index*6+4]=(i+1)*size+(j+1);
			triangles[index*6+5]=(i+1)*size+(j+0);
			index++;
		}
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.RecalculateNormals ();
	


	}

	void Shallow_Wave()
	{
        float i_minus;
        float i_plus;
        float j_minus;
        float j_plus;
		float rate = 0.005f;
		float damping = 0.999f;
        for (int i = 1; i < size-1; i++)
        {
            for (int j = 1; j < size-1; j++)
            {
                if (i == 0)
                {
                    i_minus = h[i, j];
                }
                else
                {
                    i_minus = h[i - 1, j];
                }
                if (i == size - 1)
                {
                    i_plus = h[i, j];
                }
                else
                {
                    i_plus = h[i + 1, j];
                }
                if (j == 0)
                {
                    j_minus = h[i, j];
                }
                else
                {
                    j_minus = h[i, j - 1];
                }
                if (j == size - 1)
                {
                    j_plus = h[i, j];
                }
                else
                {
                    j_plus = h[i, j + 1];
                }
                float hi = h[i, j];
                float oldh = old_h[i, j];
                //new_h[i, j] = hi + damping * (hi - oldh) + (h[i-1,j] + h[i+1,j] + h[i,j-1] + h[i,j+1] - 4 * h[i, j]) * rate;
                new_h[i, j] = hi + damping * (hi - oldh) + (h[i - 1, j] + h[i + 1, j] + h[i, j - 1] + h[i, j + 1] - 4 * h[i, j]) * rate;
                old_h[i, j] = h[i, j];
                h[i, j] = new_h[i, j];
            }
        }

	}

	// Update is called once per frame
	void Update () 
	{
		Mesh mesh = GetComponent<MeshFilter> ().mesh;
		Vector3[] vertices = mesh.vertices;

		//Step 1: Copy vertices.y into h
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                h[i, j] = mesh.vertices[i * size + j].y;
                if(Input.GetKey("r"))
                {
                    int a = Random.Range(0, size-1);
                    int b = Random.Range(0, size-1);
                    float m = Random.Range(0.05f, 0.1f);
                    h[a, b] += m;
                }
                for (int k = 0; k < 8; k++)
                {
                    Shallow_Wave();
                }
                vertices[i * size + j].y = h[i, j];
            }
        }
		//Step 2: User interaction
	
		//Step 3: Run Shallow Wav

		//Step 4: Copy h back into mesh

		mesh.vertices = vertices;
		mesh.RecalculateNormals ();

	}
}
