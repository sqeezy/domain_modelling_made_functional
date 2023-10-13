namespace OrderTaking.Domain

open OrderTaking.Generic

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


type AcknowledgeOrder =
    CreateOrderAcknowledgementLetter -> SendOrderAcknowledgement -> PricedOrder -> Async<OrderAcknowledgementSent option>

type CreateEvents = PricedOrder -> PlaceOrderEvent list

module UnitQuantity =
    let create =
        function
        | over when over > 1000 -> Error "Unit quantity cannot be over 1000"
        | under when under < 1 -> Error "Unit quantity cannot be under 1"
        | good -> Ok(UnitQuantity good)
