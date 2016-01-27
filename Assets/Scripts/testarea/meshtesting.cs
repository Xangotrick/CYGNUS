using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class meshtesting : MonoBehaviour
{
    public MeshBuilder meshbuildeeee;
    public static bool work = true;

    void Start()
    {
        meshbuildeeee = new MeshBuilder();
    }
    // Use this for initialization
    public string[, ,] amap;
    public void LoadUpdate(string[,,] mappy)
    {
        amap = new string[8, 128, 8];
        int size = amap.GetLength(0);
        int sizey = amap.GetLength(1);
        amap = mappy;

        List<string> cleanuplist = new List<string>();
        cleanuplist.Add("quartz");
        cleanuplist.Add("basaltsource");
        for (int kx = 0; kx < size; kx++)
        {
            for (int ky = 0; ky < 128; ky++)
            {
                for (int kz = 0; kz < size; kz++)
                {
                    foreach (string astring in cleanuplist)
                    {
                        if (amap[kx, ky, kz] == astring)
                        {
                            amap[kx, ky, kz] = "";
                        }
                    }       
                }
            }
        }
        StartCoroutine(UpdateChunk(new Vector3()));
    }

    void Update()
    {
    }

    IEnumerator UpdateChunk(Vector3 avect)
    {
        int size = amap.GetLength(0);
        int sizey = amap.GetLength(1);

        for (int kx = 0; kx < size; kx++)
        {
            for (int ky = 70; ky < 80; ky++)
            {
                for (int kz = 0; kz < size; kz++)
                {
                    if (amap[kx, ky, kz] != "")
                    {
                        Vector3 pos = new Vector3(0.5f, 0.5f, 0.5f)*0f + new Vector3(kx, ky, kz) + avect;
                        string stringhere = amap[kx, ky, kz];
                        bool[] thebool = new bool[6];
                        if (kx != 0)
                        {
                            if (kx != size - 1)
                            {
                                string next = amap[kx + 1, ky, kz];
                                thebool[0] = (next == "");
                                next = amap[kx - 1, ky, kz];
                                thebool[1] = (next == "");
                            }
                            else
                            {
                                thebool[0] = true;
                                string next = amap[kx - 1, ky, kz];
                                thebool[1] = (next == "");
                            }
                        }
                        else
                        {
                            thebool[1] = true;
                            string next = amap[kx + 1, ky, kz];
                            thebool[0] = (next == "");
                        }
                        ///////////////////////
                        if (ky != 0)
                        {
                            if (ky != sizey - 1)
                            {
                                string next = amap[kx, ky + 1, kz];
                                thebool[2] = (next == "");
                                next = amap[kx, ky - 1, kz];
                                thebool[3] = (next == "");
                            }
                            else
                            {
                                thebool[2] = true;
                                string next = amap[kx, ky - 1, kz];
                                thebool[3] = (next == "");
                            }
                        }
                        else
                        {
                            thebool[3] = true;
                            string next = amap[kx, ky + 1, kz];
                            thebool[2] = (next == "");
                        }
                        ////////////////////////
                        if (kz != 0)
                        {
                            if (kz != size - 1)
                            {
                                string next = amap[kx, ky, kz + 1];
                                thebool[4] = (next == "");
                                next = amap[kx, ky, kz - 1];
                                thebool[5] = (next == "");
                            }
                            else
                            {
                                thebool[4] = true;
                                string next = amap[kx, ky, kz - 1];
                                thebool[5] = (next == "");
                            }
                        }
                        else
                        {
                            thebool[5] = true;
                            string next = amap[kx, ky, kz + 1];
                            thebool[4] = (next == "");
                        }
                        BuildCube(meshbuildeeee, pos, 1, thebool,amap[kx, ky, kz]);
                    }
                }
                yield return null;
            }
        }

        MeshFilter filter = GetComponent<MeshFilter>();
        MeshCollider coll = GetComponent<MeshCollider>();
        MeshRenderer rend = GetComponent<MeshRenderer>();

        filter.sharedMesh = meshbuildeeee.CreateMesh(worldgen.TES);
        rend.materials = meshbuildeeee.realmats;
        coll.sharedMesh = filter.sharedMesh;
        cam.controlmove = true;
    }

    public class MeshBuilder
    {
        public Material[] realmats { get; set; }
        private List<Vector3> m_Vertices = new List<Vector3>();
        public List<Vector3> Vertices { get { return m_Vertices; } }

        private List<Vector3> m_Normals = new List<Vector3>();
        public List<Vector3> Normals { get { return m_Normals; } }

        private List<Vector2> m_UVs = new List<Vector2>();
        public List<Vector2> UVs { get { return m_UVs; } }

        public List<int> m_matindex { get; set; }

        public List<int> m_Indices = new List<int>();

        public MeshBuilder()
        {
            m_matindex = new List<int>();
        }

        public void AddTriangle(int index0, int index1, int index2)
        {
            m_Indices.Add(index0);
            m_Indices.Add(index1);
            m_Indices.Add(index2);
        }

        public Mesh CreateMesh(Material[] mat)
        {
            realmats = new Material[mat.Length];

            Mesh mesh = new Mesh();

            mesh.vertices = m_Vertices.ToArray();
            mesh.triangles = m_Indices.ToArray();
            
            int numofmats = 0;
            mesh.subMeshCount = numofmats;
            List<int> indexesofmats = new List<int>();
            foreach(int anint in m_matindex)
            {
                bool inlist = false;
                foreach(int anint2 in indexesofmats)
                {
                    if(anint2 == anint)
                    {
                        inlist = true;
                    }
                }
                if(!inlist)
                {
                    indexesofmats.Add(anint);
                }
            }
            mesh.subMeshCount = indexesofmats.Count;
            List<int>[] thetris = new List<int>[indexesofmats.Count];

            for (int k = 0; k < realmats.Length; k++)
            {
                if (k < indexesofmats.Count)
                {
                    realmats[k] = mat[indexesofmats[k]];
                }
                else
                {
                    //realmats[k] = realmats[0];
                }
            }
            for (int k = 0; k < indexesofmats.Count; k++)
            {
                thetris[k] = new List<int>();
            }

            for (int k = 0; k < m_Indices.Count / 3; k++)
            {
                int rindex = 3 * k;
                int matindex = m_matindex[m_Indices[rindex]];
                for (int k2 = 0; k2 < indexesofmats.Count; k2++)
                {
                    if (indexesofmats[k2] == matindex)
                    {
                        thetris[k2].Add(m_Indices[rindex]);
                        thetris[k2].Add(m_Indices[rindex+1]);
                        thetris[k2].Add(m_Indices[rindex+2]);
                    }
                }
            }

            for (int k = 0; k < indexesofmats.Count; k++)
            {
                mesh.SetTriangles(thetris[k], k);
            }
            //Normals are optional. Only use them if we have the correct amount:
            if (m_Normals.Count == m_Vertices.Count)
                mesh.normals = m_Normals.ToArray();

            //UVs are optional. Only use them if we have the correct amount:
            if (m_UVs.Count == m_Vertices.Count)
                mesh.uv = m_UVs.ToArray();

            mesh.RecalculateBounds();

            return mesh;
        }
    }

    int[] BuildQuad(MeshBuilder meshBuilder, Vector3 loc,Vector3 Normal, Vector3 Front, float front, float side, int mati)
    {
        int[] returner = new int[] { 0, 0, 0, 0, 0, 0 };
        Vector3 Right = Vector3.Cross(Normal,Front);
        float sign = Mathf.Sign(Vector3.Dot(Front, Right));

        int mat = mati;

        meshBuilder.Vertices.Add(loc - Front * front / 2f - Right * side / 2f);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(Normal );
        meshbuildeeee.m_matindex.Add(mat);

        meshBuilder.Vertices.Add(loc + Front * front / 2f * sign - Right * side / 2f * sign);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(Normal);
        meshbuildeeee.m_matindex.Add(mat);

        meshBuilder.Vertices.Add(loc + Front * front / 2f * sign + Right * side / 2f * sign);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(Normal);
        meshbuildeeee.m_matindex.Add(mat);

        meshBuilder.Vertices.Add(loc - Front * front / 2f * sign + Right * side / 2f * sign);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(Normal);
        meshbuildeeee.m_matindex.Add(mat);

        int baseIndex = meshBuilder.Vertices.Count - 4;


        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);

        returner[0] = baseIndex;
        returner[1] = baseIndex+1;
        returner[2] = baseIndex+2;
        returner[3] = baseIndex;
        returner[4] = baseIndex+2;
        returner[5] = baseIndex+3;
        return returner;
    }

    void BuildCube(MeshBuilder meshBuilder, Vector3 pos, float size, bool[] drawn, string mat)
    {
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        int z = Mathf.RoundToInt(pos.z);

        int[] anintarr = new int[6];
        anintarr = stringtomatarr(mat);

        if (drawn[0])
        {
            BuildQuad(meshBuilder, pos + new Vector3(1, 0, 0) * size / 2f, new Vector3(1, 0, 0), new Vector3(0, 1, 0), size, size,anintarr[0]);
        }
        if (drawn[01])
        {
            BuildQuad(meshBuilder, pos + new Vector3(-1, 0, 0) * size / 2f, new Vector3(-1, 0, 0), new Vector3(0, 1, 0), size, size, anintarr[01]);
        }
        if (drawn[02])
        {
            BuildQuad(meshBuilder, pos + new Vector3(0, 1, 0) * size / 2f, new Vector3(0, 1, 0), new Vector3(0, 0, 1), size, size, anintarr[02]);
        }
        if (drawn[03])
        {
            BuildQuad(meshBuilder, pos + new Vector3(0, -1, 0) * size / 2f, new Vector3(0, -1, 0), new Vector3(0, 0, 1), size, size, anintarr[03]);
        }
        if (drawn[04])
        {
            BuildQuad(meshBuilder, pos + new Vector3(0, 0, 1) * size / 2f, new Vector3(0, 0, 1), new Vector3(1, 0, 0), size, size, anintarr[04]);
        }
        if (drawn[05])
        {
            BuildQuad(meshBuilder, pos + new Vector3(0, 0, -1) * size / 2f, new Vector3(0, 0, -1), new Vector3(1, 0, 0), size, size, anintarr[05]);
        }
    }

    int[] stringtomatarr(string astring)
    {
        int[] returner = new int[6];

        for (int k = 0; k < 6; k++)
        {
            switch (astring)
            {
                case "grass":
                    returner[k] = 1;
                    if (k == 2)
                    {
                        returner[k] = 2;
                    }
                    break;
                case "dirt":
                    returner[k] = 1;
                    break;
                case "stone":
                    returner[k] = 0;
                    break;
                case "woodlog":
                    returner[k] = 3;
                    if (k == 2 || k == 3)
                    {
                        returner[k] = 4;
                    }
                    break;
                case "woodleaf":
                    returner[k] = 5;
                    break;
                case "marbleraw":
                    returner[k] = 8;
                    break;
                case "orecopper":
                    returner[k] = 6;
                    break;
                case "oreiron":
                    returner[k] = 7;
                    break;
                case "bedrock":
                    returner[k] = 9;
                    break;
                case "orecoal":
                    returner[k] = 10;
                    break;
                case "orealuminium":
                    returner[k] = 11;
                    break;
                default:
                    returner[k] = 0;
                    break;
            }
        }

        return returner;
    }

    public void DestroyCube(int kx, int ky, int kz)
    {
        print("blip");
        float dist = 0.5f;
        Vector3 pos = new Vector3(kx, ky, kz);
        List<int> alist = new List<int>();
        List<int> alistorder = new List<int>();
        for (int k = 0; k < meshbuildeeee.m_Indices.Count / 3; k++)
        {
            bool take = true;
            Vector3 A = meshbuildeeee.Vertices[meshbuildeeee.m_Indices[3 * k]];
            Vector3 B = meshbuildeeee.Vertices[meshbuildeeee.m_Indices[3 * k + 1]];
            Vector3 C = meshbuildeeee.Vertices[meshbuildeeee.m_Indices[3 * k + 2]];
            Vector3 vect = A;
            bool isone = (vect == pos + new Vector3(0 + dist, 0 + dist, 0 + dist)) || (vect == pos + new Vector3(0 + dist, 0 + dist, 0 - dist)) || (vect == pos + new Vector3(0 - dist, 0 + dist, 0 + dist)) || (vect == pos + new Vector3(0 - dist, 0 + dist, 0 - dist)) || (vect == pos + new Vector3(0, 0 - dist, 0)) || (vect == pos + new Vector3(0 + dist, 0 - dist, 0 + dist)) || (vect == pos + new Vector3(0 + dist, 0 - dist, 0 - dist)) || (vect == pos + new Vector3(0 - dist, 0 - dist, 0 + dist)) || (vect == pos + new Vector3(0 - dist, 0 - dist, 0 - dist));
            if (!isone || Vector3.Dot(A - pos, meshbuildeeee.Normals[meshbuildeeee.m_Indices[3 * k]]) < 0)
            {
                take = false;
                goto vuva;
            }
            else
            {
                //print(vect - pos);
            }
            vect = B;
            isone = (vect == pos + new Vector3(0 + dist, 0 + dist, 0 + dist)) || (vect == pos + new Vector3(0 + dist, 0 + dist, 0 - dist)) || (vect == pos + new Vector3(0 - dist, 0 + dist, 0 + dist)) || (vect == pos + new Vector3(0 - dist, 0 + dist, 0 - dist)) || (vect == pos + new Vector3(0, 0 - dist, 0)) || (vect == pos + new Vector3(0 + dist, 0 - dist, 0 + dist)) || (vect == pos + new Vector3(0 + dist, 0 - dist, 0 - dist)) || (vect == pos + new Vector3(0 - dist, 0 - dist, 0 + dist)) || (vect == pos + new Vector3(0 - dist, 0 - dist, 0 - dist));
            if (!isone || Vector3.Dot(B - pos, meshbuildeeee.Normals[meshbuildeeee.m_Indices[3 * k + 1]]) < 0)
            {
                take = false;
                goto vuva;
            }
            else
            {
                //print(vect - pos);
            }
            vect = C;
            isone = (vect == pos + new Vector3(0 + dist, 0 + dist, 0 + dist)) || (vect == pos + new Vector3(0 + dist, 0 + dist, 0 - dist)) || (vect == pos + new Vector3(0 - dist, 0 + dist, 0 + dist)) || (vect == pos + new Vector3(0 - dist, 0 + dist, 0 - dist)) || (vect == pos + new Vector3(0, 0 - dist, 0)) || (vect == pos + new Vector3(0 + dist, 0 - dist, 0 + dist)) || (vect == pos + new Vector3(0 + dist, 0 - dist, 0 - dist)) || (vect == pos + new Vector3(0 - dist, 0 - dist, 0 + dist)) || (vect == pos + new Vector3(0 - dist, 0 - dist, 0 - dist));
            if (!isone || Vector3.Dot(C - pos, meshbuildeeee.Normals[meshbuildeeee.m_Indices[3 * k + 2]]) < 0)
            {
                take = false;
                goto vuva;
            }
            else
            {
                //print(vect - pos);
            }

            if (take)
            {
                //print(meshbuildeeee.m_Indices[3 * k]);
                //print(meshbuildeeee.m_Indices[3 * k+1]);
                //print(meshbuildeeee.m_Indices[3 * k+2]);
                alist.Add(meshbuildeeee.m_Indices[3 * k]);
                alist.Add(meshbuildeeee.m_Indices[3 * k + 1]);
                alist.Add(meshbuildeeee.m_Indices[3 * k + 2]);
                alist.Sort();
            }
            else
            {
            }
        vuva: ;
            //yield return null;
        }
        if (alist.Count != 0)
        {
            foreach (int anint in alist)
            {
                if (alistorder.Count == 0)
                {
                    alistorder.Add(anint);
                }
                else
                {
                    if (alistorder[alistorder.Count - 1] != anint)
                    {
                        alistorder.Add(anint);
                    }
                }
            }


            for (int ki = alistorder.Count - 1; ki >= 0; ki--)
            {
                meshbuildeeee.Vertices.RemoveAt(alistorder[ki]);
                meshbuildeeee.Normals.RemoveAt(alistorder[ki]);
                meshbuildeeee.UVs.RemoveAt(alistorder[ki]);
                meshbuildeeee.m_matindex.RemoveAt(alistorder[ki]);
            }

            List<int> removalsoftris = new List<int>();
            for (int k = (meshbuildeeee.m_Indices.Count / 3) - 1; k >= 0; k--)
            {
                
                int iA = meshbuildeeee.m_Indices[3 * k];
                int iB = meshbuildeeee.m_Indices[3 * k + 1];
                int iC = meshbuildeeee.m_Indices[3 * k + 2];

                int min = alistorder[0];
                int minus = 0;
                bool falsee = false;
                foreach (int anint in alistorder)
                {
                    if (iA == anint || iB == anint || iC == anint)
                    {
                        falsee = true;
                    }
                }
                if (falsee)
                {
                    meshbuildeeee.m_Indices.RemoveAt(3 * k + 2);
                    meshbuildeeee.m_Indices.RemoveAt(3 * k + 1);
                    meshbuildeeee.m_Indices.RemoveAt(3 * k + 0);
                    removalsoftris.Add(k);
                    //print(k);
                }
                else
                {
                    for (int ka = 0; ka < alistorder.Count; ka++)
                    {
                        minus = ka;
                        if (meshbuildeeee.m_Indices[3 * k + 2] < alistorder[ka])
                        {
                            break;
                        }
                    }

                    if (meshbuildeeee.m_Indices[3 * k + 2] > alistorder[alistorder.Count - 1])
                    {
                        minus = alistorder.Count;
                    }

                    meshbuildeeee.m_Indices[3 * k] -= minus;
                    meshbuildeeee.m_Indices[3 * k + 1] -= minus;
                    meshbuildeeee.m_Indices[3 * k + 2] -= minus;
                }
            }
            
            int size = amap.GetLength(0);
            int sizey = amap.GetLength(1);
            string stringhere = amap[kx, ky, kz];
            bool[] thebool = new bool[6];
            int[] thestring = new int[6];
            if (kx != 0)
            {
                if (kx != size - 1)
                {
                    string next = amap[kx + 1, ky, kz];
                    thebool[0] = (next == "");
                    next = amap[kx - 1, ky, kz];
                    thebool[1] = (next == "");
                }
                else
                {
                    thebool[0] = true;
                    string next = amap[kx - 1, ky, kz];
                    thebool[1] = (next == "");
                }
            }
            else
            {
                thebool[1] = true;
                string next = amap[kx + 1, ky, kz];
                thebool[0] = (next == "");
            }
            ///////////////////////
            if (ky != 0)
            {
                if (ky != sizey - 1)
                {
                    string next = amap[kx, ky + 1, kz];
                    thebool[2] = (next == "");
                    next = amap[kx, ky - 1, kz];
                    thebool[3] = (next == "");
                }
                else
                {
                    thebool[2] = true;
                    string next = amap[kx, ky - 1, kz];
                    thebool[3] = (next == "");
                }
            }
            else
            {
                thebool[3] = true;
                string next = amap[kx, ky + 1, kz];
                thebool[2] = (next == "");
            }
            ////////////////////////
            if (kz != 0)
            {
                if (kz != size - 1)
                {
                    string next = amap[kx, ky, kz + 1];
                    thebool[4] = (next == "");
                    next = amap[kx, ky, kz - 1];
                    thebool[5] = (next == "");
                }
                else
                {
                    thebool[4] = true;
                    string next = amap[kx, ky, kz - 1];
                    thebool[5] = (next == "");
                }
            }
            else
            {
                thebool[5] = true;
                string next = amap[kx, ky, kz + 1];
                thebool[4] = (next == "");
            }
            if (work)
            {

                if (!thebool[0])
                {
                    thestring = stringtomatarr(amap[kx+1, ky, kz]);
                    BuildQuad(meshbuildeeee, pos + new Vector3(+1, 0, 0) * 1 / 2f, new Vector3(-1, 0, 0), new Vector3(0, 1, 0), 1, 1, thestring[0]);
                }
                if (!thebool[01])
                {
                    thestring = stringtomatarr(amap[kx-1, ky, kz]);
                    BuildQuad(meshbuildeeee, pos + new Vector3(-1, 0, 0) * 1 / 2f, new Vector3(1, 0, 0), new Vector3(0, 1, 0), 1, 1, thestring[1]);
                }
                if (!thebool[02])
                {
                    thestring = stringtomatarr(amap[kx, ky+1, kz]);
                    BuildQuad(meshbuildeeee, pos + new Vector3(0, +1, 0) * 1 / 2f, new Vector3(0, -1, 0), new Vector3(0, 0, 1), 1, 1, thestring[02]);
                }
                if (!thebool[03])
                {
                    thestring = stringtomatarr(amap[kx, ky-1, kz]);
                    BuildQuad(meshbuildeeee, pos + new Vector3(0, -1, 0) * 1 / 2f, new Vector3(0, 1, 0), new Vector3(0, 0, 1), 1, 1, thestring[03]);
                }
                if (!thebool[04])
                {
                    thestring = stringtomatarr(amap[kx, ky, kz+1]);
                    BuildQuad(meshbuildeeee, pos + new Vector3(0, 0, +1) * 1 / 2f, new Vector3(0, 0, -1), new Vector3(1, 0, 0), 1, 1, thestring[04]);
                }
                if (!thebool[05])
                {
                    thestring = stringtomatarr(amap[kx, ky, kz-1]);
                    BuildQuad(meshbuildeeee, pos + new Vector3(0, 0, -1) * 1 / 2f, new Vector3(0, 0, 1), new Vector3(1, 0, 0), 1, 1, thestring[05]);
                }
            }
            


            amap[kx, ky, kz] = "";
            MeshFilter filter = GetComponent<MeshFilter>();
            MeshCollider coll = GetComponent<MeshCollider>();
            MeshRenderer rend = GetComponent<MeshRenderer>();

            filter.sharedMesh = meshbuildeeee.CreateMesh(worldgen.TES);
            rend.materials = meshbuildeeee.realmats;
            coll.sharedMesh = filter.sharedMesh;

            List<int> saveme = new List<int>();
            foreach (int anint in meshbuildeeee.m_matindex)
            {
                saveme.Add(anint);
            }


            meshbuildeeee = new MeshBuilder();
            
            foreach (Vector3 avect in filter.sharedMesh.vertices)
            {
                meshbuildeeee.Vertices.Add(avect);
            }
            foreach (Vector3 avect in filter.sharedMesh.normals)
            {
                meshbuildeeee.Normals.Add(avect);
            }
            foreach (Vector3 avect in filter.sharedMesh.uv)
            {
                meshbuildeeee.UVs.Add(avect);
            }
            foreach (int anint in filter.sharedMesh.triangles)
            {
                meshbuildeeee.m_Indices.Add(anint);
            }
            foreach (int anint in saveme)
            {
                meshbuildeeee.m_matindex.Add(anint);
            }
            
        }

    }
}
