Pickup Hockey
==============

[![CodeQL](https://github.com/morrisonbrett/pickuphockey/actions/workflows/codeql.yml/badge.svg)](https://github.com/morrisonbrett/pickuphockey/actions/workflows/codeql.yml)

A project to buy, sell, and manage "spots" in a pickup hockey session.  In 2004, our commissioner, John Bryan, started a friendly pickup game on Friday mornings.  It's been such a great, well run, enjoyable hockey experience for everyone, and a few years ago we added a Wednesday session.  We play at [Toyota Sports Performance Center](https://www.toyotasportsperformancecenter.com/) in El Segundo, California, where the NHL Los Angeles Kings practice.

For years, we have has been managing the buy / sell list via email.  About a week before each skating session, the commissioner sends an email as the "buy / sell thread".  Regulars and substitutes add their names as a buyer or seller, like:

    Bill sells to Bob
    TBD sells to Joe

And each player that responds is expected to modify the thread accordingly, and plug their name in or add it to the list.  There are 20 spots, and many more on the mailing list.  Inevitably, someone always manages to botch it and confuse everyone.

And such, the 'Pickup Hockey' project was born.  This project allows users to buy / sell their spots with a few clicks, and integrates with PayPal for buyers to pay sellers.  Every action generates an email and a timestamped activity log.  Admins can take actions on behalf of other users.

The project is built on .NET / MVC / Bootstrap and uses OAuth2 to allow Google logins.  Feel free to clone, fork, and pull-request away.  Feedback is very welcome.
