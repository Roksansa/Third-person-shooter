using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

// перемещение персонажа относильно камеры
[RequireComponent(typeof(CharacterController))]
public class RelativeMov : MonoBehaviour{
  public float rotSpeed = 20f;
  public float moveSpeed = 6.0f;

  [SerializeField] private Transform target;

  //прыжки
  public float jumpSpeed = 15f;

  public float gravity = -9.8f;
  public float terminalVelocity = -10f; //скорость падения
  public float minFall = -1.5f;
  private CharacterController controller;
  private ControllerColliderHit contact;
  private float vertSpeed;

  // anim
  private Animator animator;

  //столкновения с объектами
  public float pushForce = 3f;

  // PointClickMovement

  public float deceleration = 20.0f;
  public float targetBuffer = 6.0f;
  private float curSpeed = 0f;
  private Vector3 targetPos = Vector3.zero;
  private bool lastStep = false;

  // Use this for initialization
  void Start() {
    controller = GetComponent<CharacterController>();
    vertSpeed = minFall;
    animator = GetComponent<Animator>();
  }


  // Update is called once per frame
  void Update() {
    if (!UIController.PauseGame) {
      if (TPSCamera.Mode2D) {
        Move2D();
      }
      else {
        Move3D();
      }
    }
  }

  private void OnControllerColliderHit(ControllerColliderHit hit) {
    //контакт с землей
    contact = hit;

    //определение возможности столкновения
    Rigidbody body = hit.collider.attachedRigidbody;
    if (body != null && !body.isKinematic) {
      body.velocity = hit.moveDirection*pushForce;
    }
  }

  private void Move3D() {
    Vector3 mov = Vector3.zero;
    float horInput = Input.GetAxis("Horizontal");
    float verInput = Input.GetAxis("Vertical");
    if (horInput != 0 || verInput != 0) {
      mov.x = horInput*moveSpeed;
      mov.z = verInput*moveSpeed;
      mov = Vector3.ClampMagnitude(mov, moveSpeed);
      Quaternion tmp = target.rotation;
      target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
      mov = target.TransformDirection(mov);
      target.rotation = tmp;
      tmp = Quaternion.LookRotation(mov);

      transform.rotation = Quaternion.Lerp(transform.rotation, tmp, rotSpeed*Time.deltaTime);
    }
    animator.SetFloat("Speed", mov.sqrMagnitude);
    bool hitGround = false;
    RaycastHit hit;
    // чтобы уровнять середину, персонаж сдвинут - луч бросается от стоп для лары
    float tmpH = controller.height + controller.radius;
//    Debug.Log(transform.position.x + " " + transform.position.y + " " + tmpH/2);
//    Vector3 vectmp = new Vector3(transform.position.x, transform.position.y + tmpH/2, transform.position.z);
//    Debug.Log(Physics.Raycast(vectmp, Vector3.down, out hit));
    Vector3 vectmp = transform.position;
    if (vertSpeed < 0 && Physics.Raycast(vectmp, Vector3.down, out hit)) {
      float flag = tmpH/1.9f;
      hitGround = hit.distance <= flag;
//      Debug.Log(hit.distance + "   " + flag);
    }
    if (hitGround) {
      if (Input.GetButtonDown("Jump")) {
        vertSpeed = jumpSpeed;
      }
      else {
        vertSpeed = minFall;
        animator.SetBool("Jumping", false);
      }
    }
    else {
      vertSpeed += gravity*5*Time.deltaTime;
      if (vertSpeed < terminalVelocity) {
        vertSpeed = terminalVelocity;
      }
      if (contact != null) {
        animator.SetBool("Jumping", true);
      }
      if (controller.isGrounded) {
        if (Vector3.Dot(mov, contact.normal) < 0) {
          mov = contact.normal*moveSpeed;
        }
        else {
          mov += contact.normal*moveSpeed;
        }
      }
    }
    mov.y = vertSpeed;
    mov *= Time.deltaTime;
    controller.Move(mov);
  }


  private void Move2D() {
    Vector3 mov = Vector3.zero;
    //нажимаем ли мышку чтоб поменять точку для перемещения
    if (Input.GetMouseButton(0)) {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit mouseHit;
      if (Physics.Raycast(ray, out mouseHit)) {
        //только если попали на пол
        if (mouseHit.collider.gameObject.tag == "Floor") {
          targetPos = mouseHit.point;
          curSpeed = moveSpeed;
          lastStep = false;
        }
      }
    }
    Vector3 newPos = new Vector3(transform.position.x, 0, transform.position.z);
//    Debug.Log("I TUUUUUUUUUUUUT                         " + newPos + "        " + targetPos);
//    Debug.Log(Vector3.Distance(newPos, targetPos) + "   " + curSpeed);
    //пока точка лежит ближе, чем текущая скорость шага, то замедляем
    //подбирая значение, которого будет достаточно, чтобы сделать последний шаг
    if (!lastStep) {
      while (Vector3.Distance(newPos, targetPos) < curSpeed*Time.deltaTime) {
//        Debug.Log("I TUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUT");
        curSpeed -= deceleration*Time.deltaTime;
//        Debug.Log(curSpeed);
        lastStep = true;
      }
    }
//    Debug.Log(lastStep);
    //устанавливаем координаты перемещения
    if (targetPos != Vector3.zero) {
      //новый вектор
      Vector3 adjustedPos = new Vector3(targetPos.x,
        transform.position.y, targetPos.z);
      //поворот
      Quaternion targetRot = Quaternion.LookRotation(
        adjustedPos - transform.position);
      //поворачиваем персонаж
      transform.rotation = Quaternion.Slerp(transform.rotation,
        targetRot, rotSpeed*Time.deltaTime);
      //
//      Debug.Log(mov + " 1");
//      if (lastStep) Debug.Log(mov + " 1");
      //задали мах длину передвижения
      mov = curSpeed*Vector3.forward;
//      if (lastStep) Debug.Log(mov + " 2");
      //установили изменение по осям не противореча мах длине передвижения
      mov = transform.TransformDirection(mov);
//      if (lastStep) Debug.Log(mov + " 3");
//      Debug.Log(curSpeed + " " + Vector3.Distance(targetPos, transform.position) + " " + targetBuffer + " " +
//                (Vector3.Distance(targetPos, transform.position) < targetBuffer).ToString());
      if (Vector3.Distance(adjustedPos, transform.position) < targetBuffer) {
        targetPos = Vector3.zero;
        Debug.Log(Vector3.Distance(adjustedPos, transform.position) + " 3");
      }
    }
//
//    if (mov != Vector3.zero) {
//      Debug.Log(mov + " " + mov.sqrMagnitude);
//    }
    animator.SetFloat("Speed", mov.sqrMagnitude);
 
    //в этом режиме персонаж не может прыгать, но может лететь вниз или сталкиваться
    bool hitGround = false;
    RaycastHit hit;
    // чтобы уровнять середину, персонаж сдвинут - луч бросается от стоп для лары
    float tmpH = controller.height + controller.radius;
    Vector3 vectmp = transform.position;
    if (vertSpeed < 0 && Physics.Raycast(vectmp, Vector3.down, out hit)) {
      float flag = tmpH/1.9f;
      hitGround = hit.distance <= flag;
    }
    if (hitGround) {
        vertSpeed = minFall;
        animator.SetBool("Jumping", false);
    }
    else {
      vertSpeed += gravity*5*Time.deltaTime;
      if (vertSpeed < terminalVelocity) {
        vertSpeed = terminalVelocity;
      }
      if (contact != null) {
        animator.SetBool("Jumping",true);
      }
      if (controller.isGrounded) {
        if (Vector3.Dot(mov, contact.normal) < 0) {
          mov = contact.normal*moveSpeed;
        }
        else {
          mov += contact.normal*moveSpeed;
        }
      }
    }
    mov.y = vertSpeed;
    mov *= Time.deltaTime;
    controller.Move(mov);
  }
}