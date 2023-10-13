namespace OrderTaking.Domain

open OrderTaking.Generic

// -------------------
// Input Data
// -------------------

type OrderId = Undefined
type OrderLineId = Undefined
type CustomerId = Undefined
type Address = Undefined

type EmailAdress = EmailAdress of string

// Product code related

type WidetCode = WidetCode of string
// constraint: starting with 'W' then 4 digits
type GizmoCode = GizmoCode of string
// constraint: starting with 'G' then 3 digits

type ProductCode =
    | Widget of WidetCode
    | Gizmo of GizmoCode

// Order quantity related
type UnitQuantity = private UnitQuantity of int
type KilogramQuantity = KilogramQuantity of decimal


type OrderQuantity =
    | Unit of UnitQuantity
    | Kilos of KilogramQuantity

type UnvalidatedAddress = Undefined
type ValidatedAddress = private ValidatedAddress of string

type CustomerInfo = Undefined
type UnvalidatedCustomerInfo = Undefined
type Price = Undefined
type BillingAmount = Undefined

type UnvalidatedOrderLine =
    { Id: OrderLineId
      OrderId: OrderId
      ProductCode: ProductCode
      OrderQuantity: OrderQuantity
      Price: Price }

type UnvalidatedOrder =
    { OrderId: string
      CustomerInfo: UnvalidatedCustomerInfo
      ShippingAdress: UnvalidatedAddress
      BillingAdress: UnvalidatedAddress
      OrderLines: NonEmptyList<UnvalidatedOrderLine> }
      

// -------------------
// Input Command
// -------------------

type PlaceOrder = Command<UnvalidatedOrder>

// -------------------
// Public API
// -------------------

// success output
type OrderPlaced = PricedOrder

type BillableOrderPlaced =
    { OrderId: OrderId
      BillingAdress: Address
      AmountToBill: BillingAmount }

type OrderAcknowledgementSent =
    { OrderId: OrderId
      EmailAdress: EmailAdress }

type PlaceOrderEvent =
    | OrderPlaced of OrderPlaced
    | AcknowledgementSent of OrderAcknowledgementSent option
    | BillableOrderPlaced of BillableOrderPlaced

// failure output
type ValidationError =
    { FieldName: string
      ErrorDescription: string }

type PlaceOrderError =
    | ValidationError of ValidationError list
    | OrderAlreadyExists of OrderId

// workflow
type PlaceOrderWorkflow = PlaceOrder -> Result<PlaceOrderEvent list, PlaceOrderError>