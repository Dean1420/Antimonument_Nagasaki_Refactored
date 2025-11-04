using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;

public enum Painter_BrushMode{PAINT,DECAL};
public class TexturePainter : MonoBehaviour {
	public GameObject brushCursor, brushContainer; //The cursor that overlaps the model and our container for the brushes painted
	public Camera sceneCamera, canvasCam;  //The camera that looks at the model, and the camera that looks at the canvas.
	public Sprite cursorPaint, cursorDecal; // Cursor for the differen functions 
	public RenderTexture canvasTexture; // Render Texture that looks at our Base Texture and the painted brushes
	public Material baseMaterial; // The material of our base texture (Were we will save the painted texture
    public GameObject GunPaint; //The Model for the Painting
	
	//Textures and models for painting
	public Texture2D tex;            
	public Texture2D tex2;			
	public GameObject statue;       

	//Additional textures and models for different Objects
	// public Texture2D LöweTex;
	// public Texture2D LöweTexTemp;
	// public GameObject LöweStatue;
 
	// public Texture2D SchneckeTex;
	// public Texture2D SchneckeTexTemp;
    // public GameObject SchneckeStatue;
 
	// public Texture2D JuenglingTex;
	// public Texture2D JuenglingTexTemp;
	// public GameObject JuenglingStatue;
	public GameObject tmpStatue;

	//Save textures
	public Material saveMat;
	public GameObject rendTex;

	//Sprites/Material for Decalmode
	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;
	public Sprite sprite4;
	public Sprite sprite5;
	public Sprite sprite6;
	public Material StickerPickerMat;

	//Change Skyboxes
	// public Material SkyBoxLöwe;
	// public Material SkyBoxSchnecke;
	public Material SkyBoxHub;
	// public Material SkyBoxJüngling;

	public SoundManager soundManager;

	public Painter_BrushMode mode; //Our painter mode (Paint brushes or decals)

	//Saving amount of times we saved in numbers
	int saves;
	bool saved = false;

	
	//[SerializeField] CUIColorPicker Picker;
	//[SerializeField] WebTrigger trigger;        //Control
	[SerializeField] GrabColor grabColor;      


	public float brushSize = 0.3f; //The size of our brush
	public Vector3 brushrotate = new Vector3 (0, 0 , 0);    //The rotation of our brush
	Color brushColor; //The selected color
	int brushCounter = 0, MAX_BRUSH_COUNT = 20000; //To avoid having millions of brushes
	bool saving = false; //Flag to check if we are saving the texture

	


	[SerializeField] StatueSpawnController colTest;



	void Start()
    {
	 //    if (LöweStatue.activeSelf == true)
  //       {
  //           colTest.SockelLoewe(true);
		// }
  //
  //       if (SchneckeStatue.activeSelf == true)
  //       {
  //           colTest.SockelSchnecke(true);
		// }
  //
  //       if (JuenglingStatue.activeSelf == true)
		// {
		// 	colTest.SockelJuengling(true);
		// }


		if (PlayerPrefs.GetInt("saves") == 0)				//Amount of saved textures and loads saved texture count from Playerprefs
        {
			PlayerPrefs.SetInt("saves", 0);
        }
		
	    saves = PlayerPrefs.GetInt("saves");


		mode = Painter_BrushMode.PAINT;					//Initialize the painter mode

		Debug.Log(PlayerPrefs.GetInt("saves"));

    }

    void Update()
    {
	    if (grabColor.canPaint == true)         //can paint if ray not hitting specific colliders
        {

            if (GunPaint.activeSelf)            // paintmode activated wenn Paintgun in hand
            {

                if (mode == Painter_BrushMode.PAINT)
                {
                    if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))  //More  BrushStrokes with one buttonpress
                    {
                        DoAction();
                    }
                }
                else
                {
                    if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))   // only one Sticker with one buttonpress
                    {
                        DoAction();
                    }
                }


                //if (!grabColor.savePaint)
                //{
                UpdateBrushCursor();
				
                }

            }

        //}
		//save texture
        if (grabColor.savePaint && statue != null)
        {
            SaveTexture2D();
            statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
        }
		//reset texture
        if (grabColor.resetPaint && statue != null)
        {
            restoreMaterial();
            rendTex.GetComponent<MeshRenderer>().material.mainTexture = tex;                        
            statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
            grabColor.resetPaint = false;
        }
		//load texture
        if (grabColor.loadSave)
        {
            restoreMaterial();                                                                      
            rendTex.GetComponent<MeshRenderer>().material.mainTexture = tex2;
            statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
            grabColor.loadSave = false;
        }


		//Determine the current painter mode (paint or decal)= based on UI interaction
        if (grabColor.decalMode)
        {
            mode = Painter_BrushMode.DECAL;                                                        
            SetBrushMode(Painter_BrushMode.DECAL);
        }
        else
        {
            mode = Painter_BrushMode.PAINT;                                                     
            SetBrushMode(Painter_BrushMode.PAINT);
        }

		//Determine the current decal sprite based on UI interaction
        if (grabColor.decalcounter == 0)
        {
            cursorDecal = sprite1;
            StickerPickerMat.mainTexture = sprite1.texture;
        }

        else if (grabColor.decalcounter == 1)
        {
            cursorDecal = sprite2;
            StickerPickerMat.mainTexture = sprite2.texture;
        }

        else if (grabColor.decalcounter == 2)
        {
            cursorDecal = sprite3;
            StickerPickerMat.mainTexture = sprite3.texture;
        }

        else if (grabColor.decalcounter == 3)
        {
            cursorDecal = sprite4;
            StickerPickerMat.mainTexture = sprite4.texture;
        }

        else if (grabColor.decalcounter == 4)
        {
            cursorDecal = sprite5;
            StickerPickerMat.mainTexture = sprite5.texture;
        }

		else if (grabColor.decalcounter == 5)
		{
			cursorDecal = sprite6;
			StickerPickerMat.mainTexture = sprite6.texture;
		}

    }

    public void Init()
    {
	    // grabColor.savePaint = true;
	    StartCoroutine("DelayedInit");
    }

    public IEnumerator DelayedInit()
    {
	    var time = 0f;

	    while (time >= 1f)
	    {
		    time += Time.deltaTime;
		    yield return new WaitForEndOfFrame();
	    }
	    SaveTexture2D();
	    statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
	    yield return null;
    }

    public void SaveTexture2D()
    {
		if(statue == null) {
            throw new FileNotFoundException("Keine Statue gefunden");
        }
        //brushCursor.SetActive(false);
            if (!saved)
            {
                saves += 1;
                PlayerPrefs.SetInt("saves", saves);             //wenn save taste gedrückt dann plus 1. funktioniert pro runde nur einmal. Jeder hat so nur einen speicherplatz für textur. wird also beim speichern immer wieder überschrieben
                saved = true;
            }
            Texture2D myTexture = toTexture2D(canvasTexture);           //brushstrokes werden auf textur übertragen
			tex2.name = "statue";
            					//speichern in assets
            saveMat.mainTexture = tex2;
            // rendTex.GetComponent<MeshRenderer>().material.mainTexture = tex2;           //statue bekommt neue textur mit gespeicherten strokes
            // statue.GetComponent<MeshRenderer>().material.mainTexture = canvasTexture;
            grabColor.savePaint = false;
            
            //auskommentiert, da nich gewünscht
			// restoreMaterial();                                                          //stroke objekte werden gelöscht.
			//brushCursor.SetActive(true);
			// statue.GetComponent<MeshRenderer>().material.mainTexture = tex2;
    }

    //The main action, instantiates a brush or decal entity at the clicked position on the UV map
    void DoAction()
	{
		if (saving)
		{
			return;
		}

		Vector3 uvWorldPosition = Vector3.zero;

		if (HitTestUVPosition(ref uvWorldPosition))
        {
            GameObject brushObj;
			brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity"));
			//Giving tag for clearing up all non parented brushObj
			brushObj.tag = "EmptyBrushEntities";

			brushColor = grabColor.board.color;

            if (mode == Painter_BrushMode.PAINT)
            {
                brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/BrushEntity")); //Paint a brush
                brushObj.GetComponent<SpriteRenderer>().color = brushColor; //Set the brush color
            }
            else
            {
                if (grabColor.decalcounter == 0)
                {
                    brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/Decal3"));
                }

                else if (grabColor.decalcounter == 1)
                {
                    brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/Decal4"));
                }

                else if (grabColor.decalcounter == 2)
                {
                    brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/Decal5"));
                }

                else if (grabColor.decalcounter == 3)
                {
                    brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/Decal6"));
                }

                else if (grabColor.decalcounter == 4)
                {
                    brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/Decal7"));
                }

				else if (grabColor.decalcounter == 5)
				{
					brushObj = (GameObject)Instantiate(Resources.Load("TexturePainter-Instances/Decal8"));
				}

			}

            brushColor.a = brushSize * 2.0f; // Brushes have alpha to have a merging effect when painted over.
			brushObj.transform.parent = brushContainer.transform; //Add the brush to our container to be wiped later
			brushObj.transform.localPosition = uvWorldPosition; //The position of the brush (in the UVMap)
            brushObj.transform.localScale = Vector3.one * brushSize;//The size of the brush
			brushObj.transform.localEulerAngles = brushrotate;

			//Debug.Log(brushObj.transform.localEulerAngles);

		}
        brushCounter++; //Add to the max brushes
		if (brushCounter >= MAX_BRUSH_COUNT)
		{
			//If we reach the max brushes available, flatten the texture and clear the brushes
			brushCursor.SetActive(false);
			saving = true;
			//Invoke("SaveTexture",0.1f);
		}
	}
	//To update at realtime the painting cursor on the mesh
	void UpdateBrushCursor() {
		Vector3 uvWorldPosition = Vector3.zero;
		if (HitTestUVPosition(ref uvWorldPosition) && !saving) {
			brushCursor.SetActive(true);
			brushCursor.transform.position = uvWorldPosition + brushContainer.transform.position;
		} else {
			brushCursor.SetActive(false);
		}
		brushCursor.transform.localScale = Vector3.one * brushSize;
	}
	//Returns the position on the texuremap according to a hit in the mesh collider
	public bool HitTestUVPosition(ref Vector3 uvWorldPosition)
	{

		//With mouse
		/*RaycastHit hit;
		Vector3 cursorPos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0.0f);
		
		Ray cursorRay=sceneCamera.ScreenPointToRay (cursorPos);
		
		if (Physics.Raycast(cursorRay,out hit,200)){
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider == null || meshCollider.sharedMesh == null)
				return false;			
			Vector2 pixelUV  = new Vector2(hit.textureCoord.x,hit.textureCoord.y);
			uvWorldPosition.x=pixelUV.x-canvasCam.orthographicSize;//To center the UV on X
			uvWorldPosition.y=pixelUV.y-canvasCam.orthographicSize;//To center the UV on Y
			uvWorldPosition.z=0.0f;
			return true;
		}
		else{		
			return false;
		}
		*/

		//With VR
		RaycastHit hit;

		if (Physics.Raycast(GunPaint.transform.position, GunPaint.transform.forward, out hit, 200))
		{
			MeshCollider meshCollider = hit.collider as MeshCollider;
			if (meshCollider == null || meshCollider.sharedMesh == null)
				return false;
			Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
			uvWorldPosition.x = pixelUV.x - canvasCam.orthographicSize;//To center the UV on X
			uvWorldPosition.y = pixelUV.y - canvasCam.orthographicSize;//To center the UV on Y
			uvWorldPosition.z = 0.0f;
			
			return true;
		}
		else {
			return false;
		}
	}
	

	
	//Changes the Cursor depending on burshmode
	 public void SetBrushMode(Painter_BrushMode brushMode){ //Sets if we are painting or placing decals
		mode = brushMode;
		brushCursor.GetComponent<SpriteRenderer> ().sprite = brushMode == Painter_BrushMode.PAINT ? cursorPaint : cursorDecal;
	}
	public void SetBrushSize() { //Sets the size of the cursor brush or decal
		//brushSize = BrushSize.value;
		brushCursor.transform.localScale = Vector3.one * brushSize;
	}

	/*public void SetColor(Color newColor)
    {
		brushColor = newColor;
    }*/

	
	
		//Savetexture local
	#if !UNITY_WEBPLAYER 
		IEnumerator SaveTextureToFile(Texture2D savedTexture){		
			brushCounter=0;
		//string fullPath=System.IO.Directory.GetCurrentDirectory()+"\\Assets\\" + "\\SavedTextures\\";
		string fullPath = "C:\\Users\\soren\\OneDrive\\Desktop" + "\\SavedTextures\\";
			System.DateTime date = System.DateTime.Now;

		string fileName = "0" + saves +StatueSpawnController.Singleton.GetCurrentName() +"_CanvasTexture.png";

		// if (SchneckeStatue.activeSelf == true)
  //       {
  //            fileName = "0" + saves + "_SchneckenTextur.png";
  //       }
  //       if (LöweStatue.activeSelf == true)
  //       {
  //           fileName = "0" + saves + "_LöwenTextur.png";
  //       }
		// if (JuenglingStatue.activeSelf == true)
		// {
		// 	fileName = "0" + saves + "_JuenglingTextur.png";
		// }


		if (!System.IO.Directory.Exists(fullPath))		
				System.IO.Directory.CreateDirectory(fullPath);
			var bytes = savedTexture.EncodeToJPG();
			System.IO.File.WriteAllBytes(fullPath+fileName, bytes);
			Debug.Log ("<color=orange>Saved Successfully!</color>"+fullPath+fileName);
			yield return null;
		}
#endif


	//Sets the base material with a our canvas texture, then removes all our brushes
	/*void SaveTexture(){		
		brushCounter=0;
		System.DateTime date = System.DateTime.Now;
		RenderTexture.active = canvasTexture;
		Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);		
		tex.ReadPixels (new Rect (0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
		tex.Apply ();
		RenderTexture.active = null;
		baseMaterial.mainTexture =tex;  //Put the painted texture as the base
		StartCoroutine(SaveTextureToFile(tex));
		foreach (Transform child in brushContainer.transform) {//Clear brushes
			Destroy(child.gameObject);
		}
		//Do you want to save the texture? This is your method!
		Invoke ("ShowCursor", 0.1f);
	}
	//Show again the user cursor (To avoid saving it to the texture)
	void ShowCursor(){	
		saving = false;
	}*/
	//restore texture
	public void restoreMaterial()
	{
		foreach (Transform child in brushContainer.transform)
		{
			Destroy(child.gameObject);
		}
	}
	//Save texture
	Texture2D toTexture2D(RenderTexture rTex)
	{
		RenderTexture.active = rTex;
		tex2 = new Texture2D(2048, 2048, TextureFormat.RGBA32, false);
		// ReadPixels looks at the active RenderTexture.
		tex2.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex2.Apply();
		saveMat.mainTexture = tex2;

		return tex;
	}
    public void MaterialToStandard()
	{
	Material currentMaterial = statue.GetComponent<MeshRenderer>().material; // Aktuelles Material des Renderers

    // Erstelle ein neues Material mit dem gewünschten Shader
    Material newMaterial = new Material(Shader.Find("Standard"));
	Debug.Log("Standard");
    // Kopiere die Eigenschaften des aktuellen Materials in das neue Material
    newMaterial.CopyPropertiesFromMaterial(currentMaterial);

    // Weise das neue Material dem Renderer zu
    statue.GetComponent<MeshRenderer>().material = newMaterial;
	}
	public void MaterialToUnlit()
	{
	Material currentMaterial = statue.GetComponent<MeshRenderer>().material; // Aktuelles Material des Renderers

	// Erstelle ein neues Material mit dem gewünschten Shader
	Material newMaterial = new Material(Shader.Find("Unlit/Texture"));

	// Kopiere die Eigenschaften des aktuellen Materials in das neue Material
	newMaterial.CopyPropertiesFromMaterial(currentMaterial);

	// Weise das neue Material dem Renderer zu
	statue.GetComponent<MeshRenderer>().material = newMaterial;
	}

	public void ResetAll()
	{
		var brushEntities = GameObject.FindGameObjectsWithTag("EmptyBrushEntities");
		foreach (var brushEntity in brushEntities)
		{
			Destroy(brushEntity);
		}
	}

    /*public void SavePNG()
	{
		RenderTexture mRt = new RenderTexture(canvasTexture.width, canvasTexture.height, canvasTexture.depth, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
		mRt.antiAliasing = canvasTexture.antiAliasing;

		var tex = new Texture2D(mRt.width, mRt.height, TextureFormat.ARGB32, false);
		cam.targetTexture = mRt;
		cam.Render();
		RenderTexture.active = mRt;

		tex.ReadPixels(new Rect(0, 0, mRt.width, mRt.height), 0, 0);
		tex.Apply();

		var path = "Assets/Textures/Rendered textures/" + fileName + ".png";
		File.WriteAllBytes(path, tex.EncodeToPNG());
		Debug.Log("Saved file to: " + path);

		DestroyImmediate(tex);

		cam.targetTexture = rt;
		cam.Render();
		RenderTexture.active = rt;

		DestroyImmediate(mRt);
	}*/




}
