Feature: Wikipedia title validation

Scenario: Validate Google's Title
	Given I want to validate the title of "Google" wikipedia page
	When I navigate to the "Google" page on wikipedia
	Then the the title of the page should be "Google - Wikipedia"

Scenario: Validate Microsoft's Title
	Given I want to validate the title of "Microsoft" wikipedia page
	When I navigate to the "Microsoft" page on wikipedia
	Then the the title of the page should be "Microsoft - Wikipedia"

Scenario: Validate Apple's Title
	Given I want to validate the title of "Apple" wikipedia page
	When I navigate to the "Apple" page on wikipedia
	Then the the title of the page should be "Apple - Wikipedia"