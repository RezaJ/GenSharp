#   Copyright 2013 RezaJ
#
#   Licensed under the Apache License, Version 2.0 (the "License");
#   you may not use this file except in compliance with the License.
#   You may obtain a copy of the License at
#
#       http://www.apache.org/licenses/LICENSE-2.0
#
#   Unless required by applicable law or agreed to in writing, software
#   distributed under the License is distributed on an "AS IS" BASIS,
#   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
#   See the License for the specific language governing permissions and
#   limitations under the License.

Feature: StartGenericServer
	To be able to have a seperate execution context in multi-threaded applications
	As a developer
	I want to have a generic server

Scenario: Initialize and terminate the GenServer
	Given I have a sample behavior
	When I start my new server
	Then Init callback should be invoked
	And Terminate callback should not be invoked yet
	And The server status should be Started
	When I stop the server
	Then Terminate callback should be invoked 
	Then The server status should be Stopped

@terminate_server
Scenario: Simple calculator
	Given I have a sample behavior
	And I start my new server
	When I do a call on method Add by value 200 the reply is 200
	And  I do a post on method Multiply by value 2
	And I do a post on method Add by value 50 
	Then I do a call on method Equals and the reply is 450

@terminate_server
Scenario: Error handling
	Given I have a sample behavior
	And I start my new server
	When I do a post on method Add by value 600 
	And  I do a post on method Divide by value 0
	Then The Error handler gets called for method Divide with message 0 and error Attempted to divide by zero.
	When  I do a call on method MultiplyAndReturn by value 999999999 the reply is 600
	Then The Error handler gets called for method MultiplyAndReturn with message 999999999 and error Overflow Exception.
	And I do a call on method Equals and the reply is 600

@terminate_server
Scenario: Collect unhandled requests
	Given I have a sample behavior
	And I start my new server
	When I do a post on method Add by value 600 
	And  I do a post on method Power by value 2
	Then The Info handler gets called for method Power with message 2
	When  I do a call on method Multiply by value 999999999 the reply is 600
	Then The Info handler gets called for method Multiply with message 999999999
	And I do a call on method Equals and the reply is 600