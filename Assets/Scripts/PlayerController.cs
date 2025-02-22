using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbody;
    public float rotationSpeed = 10f;
    public float walkSpeed = 2f;  // Скорость ходьбы
    public float runSpeed = 5f;   // Скорость бега

    private bool isRunning = false; // Для отслеживания, бежит ли игрок

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 directionVector = new Vector3(-v, 0, h);

        // Определяем скорость в зависимости от того, зажат ли Shift
        float currentSpeed = walkSpeed; // Если не бежим, скорость равна ходьбе

        if (directionVector.magnitude > 0.05f)  // Если персонаж двигается
        {
            if (Input.GetKey(KeyCode.LeftShift))  // Если Shift зажат и персонаж двигается
            {
                currentSpeed = runSpeed;
            }

            // Поворачиваем персонажа в сторону движения
            Quaternion targetRotation = Quaternion.LookRotation(directionVector); // Строим цель поворота
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Устанавливаем анимацию в зависимости от скорости
            animator.SetFloat("speed", directionVector.magnitude);

            // Применяем движение с соответствующей скоростью
            rigidbody.linearVelocity = directionVector.normalized * currentSpeed;

            // Проверяем нажатие шифта для бега
            if (Input.GetKeyDown(KeyCode.LeftShift) && directionVector.magnitude > 0.05f)
            {
                animator.SetTrigger("Running");  // Активируем анимацию бега
                isRunning = true;
            }
        }
        else
        {
            // Если персонаж не двигается
            animator.SetFloat("speed", 0);
            rigidbody.linearVelocity = Vector3.zero; // Останавливаем движение

            // Сбрасываем вращение, если персонаж не двигается
            rigidbody.angularVelocity = Vector3.zero; // Останавливаем вращение

            // Если персонаж стоял, но теперь отпускает Shift
            if (isRunning && !Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetTrigger("StopRunning"); // Останавливаем анимацию бега
                isRunning = false;
            }
        }
    }
}




















