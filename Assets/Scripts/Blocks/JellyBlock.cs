using UnityEngine;

public class JellyBlock : BaseBlock
{
    [Header("Jelly Settings")]
    [SerializeField] private float wobbleSpeed = 5f;
    [SerializeField] private float wobbleIntensity = 0.15f;
    [SerializeField] private float wobbleDecay = 3f;
    [SerializeField] private float impactMultiplier = 0.5f;
    [SerializeField] private float jellyFrequency = 3f;
    [SerializeField] private float jellyAmplitude = 0.03f;
    
    [Header("Squash & Stretch")]
    [SerializeField] private float maxSquash = 1.3f;
    [SerializeField] private float minSquash = 0.7f;
    [SerializeField] private float squashSpeed = 8f;
    [SerializeField] private float squashDecay = 5f;
    
    [Header("Bounce Settings")]
    [SerializeField] private float bounciness = 0.6f;
    
    private SpriteRenderer spriteRenderer;
    private MaterialPropertyBlock propertyBlock;
    
    private float currentWobbleX;
    private float currentWobbleZ;
    private float targetSquashX = 1f;
    private float targetSquashY = 1f;
    private float currentSquashX = 1f;
    private float currentSquashY = 1f;
    
    private Vector2 lastVelocity;
    private bool isInitialized;
    
    // Shader property IDs for performance
    private static readonly int WobbleXProperty = Shader.PropertyToID("_WobbleX");
    private static readonly int WobbleZProperty = Shader.PropertyToID("_WobbleZ");
    private static readonly int WobbleSpeedProperty = Shader.PropertyToID("_WobbleSpeed");
    private static readonly int WobbleIntensityProperty = Shader.PropertyToID("_WobbleIntensity");
    private static readonly int SquashXProperty = Shader.PropertyToID("_SquashX");
    private static readonly int SquashYProperty = Shader.PropertyToID("_SquashY");
    private static readonly int JellyFrequencyProperty = Shader.PropertyToID("_JellyFrequency");
    private static readonly int JellyAmplitudeProperty = Shader.PropertyToID("_JellyAmplitude");
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("JellyBlock requires a SpriteRenderer component!");
            return;
        }
        
        propertyBlock = new MaterialPropertyBlock();
        
        // Set up physics material for bounciness
        SetupBouncyPhysics();
        
        // Initialize shader properties
        UpdateShaderProperties();
        isInitialized = true;
    }
    
    void SetupBouncyPhysics()
    {
        Collider2D col = Collider;
        if (col != null)
        {
            PhysicsMaterial2D jellyMaterial = new PhysicsMaterial2D("JellyMaterial");
            jellyMaterial.bounciness = bounciness;
            jellyMaterial.friction = 0.3f;
            col.sharedMaterial = jellyMaterial;
        }
    }
    
    void Update()
    {
        if (!isInitialized) return;
        
        // Decay wobble over time
        currentWobbleX = Mathf.Lerp(currentWobbleX, 0f, wobbleDecay * Time.deltaTime);
        currentWobbleZ = Mathf.Lerp(currentWobbleZ, 0f, wobbleDecay * Time.deltaTime);
        
        // Decay squash back to normal
        targetSquashX = Mathf.Lerp(targetSquashX, 1f, squashDecay * Time.deltaTime);
        targetSquashY = Mathf.Lerp(targetSquashY, 1f, squashDecay * Time.deltaTime);
        
        // Smooth squash interpolation
        currentSquashX = Mathf.Lerp(currentSquashX, targetSquashX, squashSpeed * Time.deltaTime);
        currentSquashY = Mathf.Lerp(currentSquashY, targetSquashY, squashSpeed * Time.deltaTime);
        
        // Add continuous subtle wobble based on velocity
        if (Rigidbody != null && Rigidbody.bodyType == RigidbodyType2D.Dynamic)
        {
            Vector2 velocity = Rigidbody.linearVelocity;
            
            // Add velocity-based wobble
            float velocityWobble = velocity.magnitude * 0.01f;
            currentWobbleX += velocity.x * velocityWobble * Time.deltaTime;
            currentWobbleZ += velocity.y * velocityWobble * Time.deltaTime;
            
            lastVelocity = velocity;
        }
        
        UpdateShaderProperties();
    }
    
    void UpdateShaderProperties()
    {
        if (spriteRenderer == null || propertyBlock == null) return;
        
        spriteRenderer.GetPropertyBlock(propertyBlock);
        
        propertyBlock.SetFloat(WobbleXProperty, currentWobbleX);
        propertyBlock.SetFloat(WobbleZProperty, currentWobbleZ);
        propertyBlock.SetFloat(WobbleSpeedProperty, wobbleSpeed);
        propertyBlock.SetFloat(WobbleIntensityProperty, wobbleIntensity);
        propertyBlock.SetFloat(SquashXProperty, currentSquashX);
        propertyBlock.SetFloat(SquashYProperty, currentSquashY);
        propertyBlock.SetFloat(JellyFrequencyProperty, jellyFrequency);
        propertyBlock.SetFloat(JellyAmplitudeProperty, jellyAmplitude);
        
        spriteRenderer.SetPropertyBlock(propertyBlock);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isInitialized) return;
        
        // Calculate impact force
        float impactForce = collision.relativeVelocity.magnitude;
        Vector2 impactNormal = collision.contacts[0].normal;
        
        // Transform impact normal to local space to account for block rotation
        Vector2 localImpactNormal = transform.InverseTransformDirection(impactNormal);
        
        // Add wobble based on impact direction and force (in local space)
        currentWobbleX += localImpactNormal.x * impactForce * impactMultiplier;
        currentWobbleZ += localImpactNormal.y * impactForce * impactMultiplier;
        
        // Apply squash and stretch based on local impact direction
        if (Mathf.Abs(localImpactNormal.y) > Mathf.Abs(localImpactNormal.x))
        {
            // Vertical impact (relative to block) - squash vertically, stretch horizontally
            float squashAmount = Mathf.Clamp(impactForce * 0.05f, 0f, maxSquash - 1f);
            targetSquashX = Mathf.Clamp(1f + squashAmount, minSquash, maxSquash);
            targetSquashY = Mathf.Clamp(1f - squashAmount * 0.5f, minSquash, maxSquash);
        }
        else
        {
            // Horizontal impact (relative to block) - squash horizontally, stretch vertically
            float squashAmount = Mathf.Clamp(impactForce * 0.05f, 0f, maxSquash - 1f);
            targetSquashX = Mathf.Clamp(1f - squashAmount * 0.5f, minSquash, maxSquash);
            targetSquashY = Mathf.Clamp(1f + squashAmount, minSquash, maxSquash);
        }
    }
    
    public override void OnCranePickup()
    {
        base.OnCranePickup();
        
        // Stretch effect when picked up
        targetSquashX = minSquash;
        targetSquashY = maxSquash;
        currentWobbleX = 0.5f;
        currentWobbleZ = 0.5f;
    }
    
    public override void OnCraneDrop()
    {
        base.OnCraneDrop();
        
        // Squash effect when dropped
        targetSquashX = maxSquash;
        targetSquashY = minSquash;
        currentWobbleX = 0.3f;
        currentWobbleZ = -0.3f;
    }
    
    public override void OnCraneUpdate()
    {
        base.OnCraneUpdate();
        
        // Subtle wobble while being held
        if (Rigidbody != null)
        {
            Vector2 velocity = Rigidbody.linearVelocity;
            currentWobbleX += velocity.x * 0.001f;
            currentWobbleZ += velocity.y * 0.001f;
        }
    }
}
