type Undefined = exn

type CustomerId = CustomerId of int
type OrderId = OrderId of int
type WidetCode = WidetCode of string
type GizmoCode = Undefined
type UnitQuantity = UnitQuantity of int
type KilogramQuantity = KilogramQuantity of decimal

type ProductCode =
    | Widget of WidetCode
    | Gizmo of GizmoCode

type OrderQuantity =
    | UnitQuantity of UnitQuantity
    | KilogramQuantity of KilogramQuantity

type CustomerInfo = Undefined
type ShippingAdress = Undefined
type BillingAdress = Undefined
type OrderLine = Undefined

type Order =
    { CustomerInfo: CustomerInfo
      ShippingAdress: ShippingAdress
      BillingAdress: BillingAdress
      OderLines: OrderLine list
      AmountToBill: Undefined }

type UnvalidatedOrder = Undefined
type ValidatedOrder = Undefined

type ValidationError =
    { FieldName : string
      ErrorDescription : string}

type ValidationResonse<'a> = Async<Result<'a, ValidationError list>>

type ValidateOrder = UnvalidatedOrder -> ValidationResonse<ValidatedOrder>

type AcknowledgementSent = Undefined
type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined

type PlaceOrrderEvents =
    { AcknowledgementSent: AcknowledgementSent
      OrderPlaced: OrderPlaced
      BillableOrderPlaced: BillableOrderPlaced }

let orderId = OrderId 1
let customerId = CustomerId 1

// (orderId = customerId) -> compile error
