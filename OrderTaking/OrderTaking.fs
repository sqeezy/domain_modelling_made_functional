namespace OrderTaking.Domain

open OrderTaking.Generic


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

type OrderId = Undefined
type OrderLineId = Undefined
type CustomerId = Undefined

type UnvalidatedAddress = Undefined
type ValidatedAddress = private ValidatedAddress of string

type CustomerInfo = Undefined
type UnvalidatedCustomerInfo = Undefined
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
      CustomerInfo: UnvalidatedCustomerInfo
      ShippingAdress: UnvalidatedAddress
      BillingAdress: UnvalidatedAddress
      OrderLines: NonEmptyList<OrderLine> }

type ValidatedOrder = {
    OrderId: OrderId
    CustomerInfo: CustomerInfo
    ShippingAdress: ValidatedAddress
    BillingAdress: ValidatedAddress
    OrderLines: NonEmptyList<OrderLine>
}

type PlaceOrderError =
    | ValidationError of ValidationError list
    | OrderAlreadyExists of OrderId

and ValidationError =
    { FieldName: string
      ErrorDescription: string }

type PlaceOrder = Command<UnvalidatedOrder>

type AcknowledgementSent = Undefined
type OrderPlaced = Undefined
type BillableOrderPlaced = Undefined

type PlaceOrderEvents =
    { AcknowledgementSent: AcknowledgementSent
      OrderPlaced: OrderPlaced
      BillableOrderPlaced: BillableOrderPlaced }

type PlaceOrderWorkflow = UnvalidatedOrder -> Result<PlaceOrderEvents, PlaceOrderError>

module UnitQuantity =
    let create =
        function
        | over when over > 1000 -> Error "Unit quantity cannot be over 1000"
        | under when under < 1 -> Error "Unit quantity cannot be under 1"
        | good -> Ok (UnitQuantity good)
