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
type Price = Undefined
type BillingAmount = Undefined

type EmailAdress = EmailAdress of string

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

type ValidatedOrderLine =
    { Id: OrderLineId
      OrderId: OrderId
      ProductCode: ProductCode
      OrderQuantity: OrderQuantity
      Price: Price }

type ValidatedOrder =
    { OrderId: OrderId
      CustomerInfo: CustomerInfo
      ShippingAdress: ValidatedAddress
      BillingAdress: ValidatedAddress
      OrderLines: NonEmptyList<ValidatedOrderLine> }

type PricedOrder = Undefined

type Order =
    | Unvalidated of UnvalidatedOrder
    | Validated of ValidatedOrder
    | Priced of PricedOrder

type PlaceOrderError =
    | ValidationError of ValidationError list
    | OrderAlreadyExists of OrderId

and ValidationError =
    { FieldName: string
      ErrorDescription: string }

type CheckedAddress = CheckedAddress of UnvalidatedAddress
type AddressValidationError = Undefined

type CheckProductCodeExists = ProductCode -> bool
type CheckAdressExists = UnvalidatedAddress -> AsyncResult<CheckedAddress, AddressValidationError>

type ValidateOrder =
    CheckProductCodeExists -> CheckAdressExists -> UnvalidatedOrder -> AsyncResult<ValidatedOrder, ValidationError list>

type GetProductPrice = ProductCode -> Price

type PricingError = PricingError of string
type PriceOrder = GetProductPrice -> ValidatedOrder -> Result<PricedOrder, PricingError>

type HtmlString = HtmlString of string

type OrderAcknowledgement =
    { EmailAdress: EmailAdress
      Letter: HtmlString }

type CreateOrderAcknowledgementLetter = PricedOrder -> HtmlString

type SendResult =
    | Sent
    | NotSent

type SendOrderAcknowledgement = OrderAcknowledgement -> Async<SendResult>

type OrderAcknowledgementSent =
    { OrderId: OrderId
      EmailAdress: EmailAdress }

type AcknowledgeOrder =
    CreateOrderAcknowledgementLetter -> SendOrderAcknowledgement -> PricedOrder -> Async<OrderAcknowledgementSent option>

type PlaceOrder = Command<UnvalidatedOrder>

type OrderPlaced = PricedOrder

type Address = Undefined

type BillableOrderPlaced =
    { OrderId: OrderId
      BillingAdress: Address
      AmountToBill: BillingAmount }

type PlaceOrderEvent =
    | OrderPlaced of OrderPlaced
    | AcknowledgementSent of OrderAcknowledgementSent option
    | BillableOrderPlaced of BillableOrderPlaced

type CreateEvents = PricedOrder -> PlaceOrderEvent list

type PlaceOrderWorkflow = UnvalidatedOrder -> Result<PlaceOrderEvent list, PlaceOrderError>

module UnitQuantity =
    let create =
        function
        | over when over > 1000 -> Error "Unit quantity cannot be over 1000"
        | under when under < 1 -> Error "Unit quantity cannot be under 1"
        | good -> Ok(UnitQuantity good)
