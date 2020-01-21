`dotnet run -- --rpcuser=user --rpcpassword=password`

This repository is looking for answers on 2 questions:

1. How much non-mixed bitcoins are entering Wasabi mixes?
2. How much non-mixed bitcoins are leaving Wasabi mixes?

Question (1) provides a comparable metric to traditional centralized mixer volumes.

Question (2) aims to create a business case for better mixing algorithm, as the change outputs those aren't get re-enqueued to another mix are proof of impatient users and lost mixing opportunity.

# Results

## [Daily Stats](https://github.com/nopara73/WasabiCjEfficiency/blob/master/DailyStats.txt)

## Monthly Stats:

```
2018-7  Fresh bitcoins: 1,      Nonremixed change: 0            Nonremixed <2 anons: 0                  Nonremixed <5 anons: 0                  Nonremixed <10 anons: 0
2018-8  Fresh bitcoins: 16,     Nonremixed change: 4            Nonremixed <2 anons: 4                  Nonremixed <5 anons: 4                  Nonremixed <10 anons: 5
2018-9  Fresh bitcoins: 49,     Nonremixed change: 11           Nonremixed <2 anons: 11                 Nonremixed <5 anons: 11                 Nonremixed <10 anons: 14
2018-10 Fresh bitcoins: 67,     Nonremixed change: 5            Nonremixed <2 anons: 5                  Nonremixed <5 anons: 5                  Nonremixed <10 anons: 5
2018-11 Fresh bitcoins: 3164,   Nonremixed change: 2504         Nonremixed <2 anons: 2504               Nonremixed <5 anons: 2504               Nonremixed <10 anons: 2504
2018-12 Fresh bitcoins: 1272,   Nonremixed change: 419          Nonremixed <2 anons: 419                Nonremixed <5 anons: 419                Nonremixed <10 anons: 419
2019-1  Fresh bitcoins: 2969,   Nonremixed change: 428          Nonremixed <2 anons: 478                Nonremixed <5 anons: 563                Nonremixed <10 anons: 672
2019-2  Fresh bitcoins: 2235,   Nonremixed change: 313          Nonremixed <2 anons: 381                Nonremixed <5 anons: 510                Nonremixed <10 anons: 609
2019-3  Fresh bitcoins: 3553,   Nonremixed change: 221          Nonremixed <2 anons: 378                Nonremixed <5 anons: 526                Nonremixed <10 anons: 611
2019-4  Fresh bitcoins: 3435,   Nonremixed change: 486          Nonremixed <2 anons: 557                Nonremixed <5 anons: 658                Nonremixed <10 anons: 777
2019-5  Fresh bitcoins: 2988,   Nonremixed change: 278          Nonremixed <2 anons: 313                Nonremixed <5 anons: 411                Nonremixed <10 anons: 542
2019-6  Fresh bitcoins: 5288,   Nonremixed change: 513          Nonremixed <2 anons: 661                Nonremixed <5 anons: 951                Nonremixed <10 anons: 1165
2019-7  Fresh bitcoins: 3754,   Nonremixed change: 255          Nonremixed <2 anons: 282                Nonremixed <5 anons: 346                Nonremixed <10 anons: 436
2019-8  Fresh bitcoins: 20328,  Nonremixed change: 1481         Nonremixed <2 anons: 2276               Nonremixed <5 anons: 4251               Nonremixed <10 anons: 10191
2019-9  Fresh bitcoins: 13456,  Nonremixed change: 506          Nonremixed <2 anons: 857                Nonremixed <5 anons: 2100               Nonremixed <10 anons: 4624
2019-10 Fresh bitcoins: 4802,   Nonremixed change: 204          Nonremixed <2 anons: 241                Nonremixed <5 anons: 328                Nonremixed <10 anons: 482
2019-11 Fresh bitcoins: 4295,   Nonremixed change: 370          Nonremixed <2 anons: 423                Nonremixed <5 anons: 524                Nonremixed <10 anons: 617
2019-12 Fresh bitcoins: 5617,   Nonremixed change: 501          Nonremixed <2 anons: 1148               Nonremixed <5 anons: 1533               Nonremixed <10 anons: 1775
2020-1  Fresh bitcoins: 4263,   Nonremixed change: 461          Nonremixed <2 anons: 1199               Nonremixed <5 anons: 1489               Nonremixed <10 anons: 1646
```

## Graphs


