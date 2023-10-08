namespace OrderTaking.Domain

type Undefined = exn

// Product code related

type WidetCode = WidetCode of string
// constraint: starting with 'W' then 4 digits
type GizmoCode = GizmoCode of string
// constraint: starting with 'G' then 3 digits

type ProductCode =
    | Widget of WidetCode
    | Gizmo of GizmoCode

// Order quantity related
type UnitQuantity = UnitQuantity of int
type KilogramQuantity = KilogramQuantity of decimal

type OrderQuantity =
    | UnitQuantity of UnitQuantity
    | KilogramQuantity of KilogramQuantity

type OrderId = Undefined
type OrderLineId = Undefined
type CustomerId = Undefined

type CustomerInfo = Undefined
type ShippingAdress = Undefined
type BillingAdress = Undefined
type Price = Undefined
type BillingAmount = Undefined

type Order =
    { Id: OrderId
      CustomerId: CustomerId
      ShippingAdress: ShippingAdress
      BillingAdress: BillingAdress
      OderLines: OrderLine list
      AmountToBill: Undefined }

and OrderLine =
    { Id: OrderLineId
      OrderId: OrderId
      ProductCode: ProductCode
      OrderQuantity: OrderQuantity
      Price: Price }

type UnvalidatedOrder =
    { OrderId: string
      CustomerInfo: CustomerInfo
      ShippingAdress: ShippingAdress
      BillingAdress: BillingAdress
      OrderLines: OrderLine list }

type ValidatedOrder = Undefined

type PlaceOrderError =
    | ValidationError of ValidationError list
    | OrderAlreadyExists of OrderId

and ValidationError =
    { FieldName: string
      ErrorDescription: string }

type AcknowledgementSent = Undefined
type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined

type PlaceOrderEvents =
    { AcknowledgementSent: AcknowledgementSent
      OrderPlaced: OrderPlaced
      BillableOrderPlaced: BillableOrderPlaced }

type PlaceOrder = UnvalidatedOrder -> Result<PlaceOrderEvents, PlaceOrderError>
