module Payments =
    type CheckNumber = CheckNumber of int
    type CardNumber = CardNumber of string

    type CardType =
        | Visa
        | MasterCard
        | AmericanExpress

    type CreditCardInfo =
        { CardNumber: CardNumber
          CardType: CardType }

    type PaymentMethod =
        | Cash
        | Check of CheckNumber
        | CreditCard of CreditCardInfo

    type PaymentAmount = PaymentAmount of decimal
    type Currency = EUR | USD | GBP

    type Payment =
        { Amount: PaymentAmount
          Currency: Currency
          Method: PaymentMethod }

    type UnpaidInvoice = unit // todo
    type PaidInvoice = unit // todo

    type PaymentError = InvalidPaymentMethod | InvalidPaymentAmount

    type PayInvoice = UnpaidInvoice -> Payment -> Result<PaidInvoice, PaymentError>
    type ConvertPaymentCurrency = Payment -> Currency -> Payment