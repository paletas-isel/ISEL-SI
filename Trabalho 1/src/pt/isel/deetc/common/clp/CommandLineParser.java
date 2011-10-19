package pt.isel.deetc.common.clp;

import java.util.Collection;
import java.util.Hashtable;
import java.util.LinkedList;
import java.util.Map;

/**
 * This class parses a command line arguments string given to the {@link #parse} method.
 * <p>
 * With the given command line string a {@link java.util.Map} is filled with the argument name being the key and the optional argument value
 * being the map value. The command line arguments map is available as the return object from {@link #parse(String)} method or as the return object from 
 * {@link #parse(String[])}.
 * </p>
 * <p>
 * The supported command line arguments String has the following format:
 * <code>{ARGUMENT_PREFIX&ltargument_name&gtARGUMENT_VALUE_SEPARATOR&ltargument_value&gt}*</code>.
 * Here is a sample command line string for the default argument prefix ({@link #DEFAULT_ARGUMENT_PREFIX} -> "-") and  argument value separator 
 * ({@link #DEFAULT_ARGUMENT_VALUE_SEPARATOR} -> " "):
 * <b>-argumet1 valueArgumen1 -argument2 valueArgument2</b>
 * </p>
 * <p>Each argument name and value are trimmed from leading and trailing spaces.</p>
 * 
 * @author Carlos Guedes, Luís Falcão e Pedro Félix
 * @version 1.0 - 19/02/2010
 */
public class CommandLineParser {
	private static interface Action{
		void action(String name, String value, Map<?, ?> map);
	}
	
	public static final String DEFAULT_ARGUMENT_PREFIX = "-";
	public static final String DEFAULT_ARGUMENT_VALUE_SEPARATOR = " ";
	public static final String DEFAULT_MULTI_VALUE_AGREGATOR = "\"";
	
	
	private final String _argumentPrefix;
	private final String _argumentValueSeparator;
	private final String _multiValueAgregator;
	
	/**
	 * Initializes a new instance of the {@link #CommandLineParser} class with default argument prefix ({@link #DEFAULT_ARGUMENT_PREFIX} and default
	 * argument value separator {@link #DEFAULT_ARGUMENT_VALUE_SEPARATOR}.
	 */
	public CommandLineParser() {
		_argumentPrefix = DEFAULT_ARGUMENT_PREFIX;
		_argumentValueSeparator = DEFAULT_ARGUMENT_VALUE_SEPARATOR;
		_multiValueAgregator = DEFAULT_MULTI_VALUE_AGREGATOR;
	}
	public CommandLineParser(String argumentPrefix) {
		_argumentPrefix = argumentPrefix;
		_argumentValueSeparator = DEFAULT_ARGUMENT_VALUE_SEPARATOR;
		_multiValueAgregator = DEFAULT_MULTI_VALUE_AGREGATOR;
	}
	
	
	/**
	 * /**
	 * Initializes a new instance of the {@link #CommandLineParser} class with the given <code>argumentPrefix</code> and <code>argumentvalueseparator</code>  
	 *
	 * @param argumentPrefix The string that prefixes the argument name
	 * @param argumentValueSeparator The string that separates the argument name from its value.
	 */
	public CommandLineParser(String argumentPrefix, String argumentValueSeparator, String multiValueAgregator) {
		_argumentPrefix = argumentPrefix;
		_argumentValueSeparator = argumentValueSeparator;
		_multiValueAgregator = multiValueAgregator;
	}
	
	
	/**
	 * Parses the given <code>commadLine</code> string array, fills the internal arguments map and returns it. 
	 * @param args The command line string to be parsed
	 * @return The parsed arguments {@link java.util.Map}
	 */
	public Map<String, String> parse(String []args) {
		if(args == null || args.length == 0)
			throw new IllegalArgumentException();
		StringBuilder sBuilder = new StringBuilder();
		boolean isInMultiValue = false;
		for(String s : args){
			if(s.contains(_multiValueAgregator))
				isInMultiValue = !isInMultiValue;
			else if(!isInMultiValue)
				sBuilder.append(' ');
			sBuilder.append(s);
		}
		return parse(sBuilder.toString());
	}
	
	/**
	 * Parses the given <code>commandLine</code> string, fills the internal arguments map and returns it. 
	 * @param commandLine The command line string to be parsed
	 * @return The parsed arguments {@link java.util.Map}
	 */
	public Map<String, String> parse(String commandLine) {
		Action singleValueAdder = new Action(){

			@Override
			public void action(String name, String value, Map<?, ?> map) {
				@SuppressWarnings("unchecked")
				Map<String,String> aux = (Map<String,String>) map;
				aux.put(name, value);				
			}
			
		};
		
		return genericParse(commandLine, new Hashtable<String,String>(), singleValueAdder, _argumentPrefix, _argumentValueSeparator, _multiValueAgregator);
	}
	
	/**
	 * Parses the given <code>commandLine</code> string, fills the given arguments map using the given Action object and returns the map. 
	 * @param commandLine The command line string to be parsed
	 * @return The parsed arguments {@link java.util.Map}
	 */
	private static <T> Map<String, T> genericParse(String commandLine, Map<String, T> argumentsMap, Action adder, String argumentPrefix, String argumentValueSeparator, String multiValueAgregator) {
		commandLine = ' ' + commandLine;
		argumentPrefix = ' ' + argumentPrefix;
		if(!commandLine.contains(argumentPrefix))
			return argumentsMap;
		if(commandLine != null && !commandLine.trim().isEmpty()) {
			String[] arguments = commandLine.substring(commandLine.indexOf(argumentPrefix)).split(argumentPrefix);
			// Iterate through the split parameters. 
			for (int i = 1; i < arguments.length; ++i) {
				String argument = arguments[i];
				int idxSeparator = argument.indexOf(argumentValueSeparator);
				String name = null;
				String value = "";
				if(idxSeparator == -1) {
					// Parameter with no value
					name = argument.trim();	
				} else {
					// Parameter with value
					// Parse the name removing (trimming) leading and trailing spaces 
					name = argument.substring(0, idxSeparator).trim();
					// Parse the value removing (trimming) leading and trailing spaces
					value = argument.substring(idxSeparator+argumentValueSeparator.length(), argument.length()).trim();
					if(value.indexOf(multiValueAgregator) == 0)
						value = value.substring(multiValueAgregator.length(), value.length() - 1).trim();
					//else if(value.contains(" "))
					//	value = value.substring(0, value.indexOf(' '));
						
				}
				adder.action(name,value,argumentsMap);
			}
		}
		return argumentsMap;
	}
	
	/**
	 * Parses the given <code>commandLine</code> string, which can have multi-value keys, fills the internal arguments map and returns it. 
	 * @param commandLine The command line string to be parsed
	 * @return The parsed arguments {@link java.util.Map}
	 */
	public Map<String, Collection<String>> parseMultiple(String commandLine) {
		Action singleValueAdder = new Action(){

			@Override
			public void action(String name, String value, Map<?, ?> map) {
				@SuppressWarnings("unchecked")
				Map<String,Collection<String>> aux = (Map<String,Collection<String>>) map;
				if(aux.containsKey(name))
					aux.get(name).add(value);
				else{
					LinkedList<String> list = new LinkedList<String>();
					list.add(value);
					aux.put(name, list);
				}
			}
			
		};
		
		return genericParse(commandLine, new Hashtable<String,Collection<String>>(), singleValueAdder, _argumentPrefix, _argumentValueSeparator, _multiValueAgregator);
	}
}
