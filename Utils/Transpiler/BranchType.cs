namespace ZerethAPI.Utils.Transpiler;

/// <summary>
/// An enumeration that specifies a branch operation.
/// </summary>
public enum BranchType
{
    /// <summary>
    /// Unconditionally transfers control to a target instruction.
    /// </summary>
    Br,

    /// <inheritdoc cref="Br"/>
    Unconditional = Br,

    /// <summary>
    /// Exits a protected region of code, unconditionally transferring control to a specific target instruction.
    /// </summary>
    Leave,

    /// <inheritdoc cref="Leave"/>
    Protected = Leave,

    /// <summary>
    /// Transfers control to a target instruction if value is false, a null reference, or zero.
    /// </summary>
    Brfalse,

    /// <inheritdoc cref="Brfalse"/>
    IfFalse = Brfalse,

    /// <inheritdoc cref="Brfalse"/>
    IfNull = Brfalse,

    /// <inheritdoc cref="Brfalse"/>
    IfZero = Brfalse,

    /// <summary>
    /// Transfers control to a target instruction if value is true, not null, or non-zero.
    /// </summary>
    Brtrue,

    /// <inheritdoc cref="Brtrue"/>
    IfTrue = Brtrue,

    /// <inheritdoc cref="Brtrue"/>
    IfNotNull = Brtrue,

    /// <inheritdoc cref="Brtrue"/>
    IfNotZero = Brtrue,

    /// <summary>
    /// Implements a jump table.
    /// </summary>
    Switch,

    /// <summary>
    /// Transfers control to a target instruction if two values are equal.
    /// </summary>
    Beq,

    /// <inheritdoc cref="Beq"/>
    Equal = Beq,

    /// <summary>
    /// Transfers control to a target instruction if the first value is greater than or equal to the second value.
    /// </summary>
    Bge,

    /// <inheritdoc cref="Bge"/>
    GreaterOrEqual = Bge,

    /// <summary>
    /// Transfers control to a target instruction if the first value is greater than the second value.
    /// </summary>
    Bgt,

    /// <inheritdoc cref="Bgt"/>
    GreaterThan = Bgt,

    /// <summary>
    /// Transfers control to a target instruction if the first value is less than or equal to the second value.
    /// </summary>
    Ble,

    /// <inheritdoc cref="Ble"/>
    LessOrEqual = Ble,

    /// <summary>
    /// Transfers control to a target instruction if the first value is less than the second value.
    /// </summary>
    Blt,

    /// <inheritdoc cref="Blt"/>
    LessThan = Blt,

    /// <summary>
    /// Transfers control to a target instruction when two unsigned integer values or unordered float values are not equal.
    /// </summary>
    Bne_Un,

    /// <inheritdoc cref="Bne_Un"/>
    NotEqual = Bne_Un,

    /// <summary>
    /// Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
    /// </summary>
    Bge_Un,

    /// <inheritdoc cref="Bge_Un"/>
    GreaterOrEqualUnordered = Bge_Un,

    /// <summary>
    /// Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
    /// </summary>
    Bgt_Un,

    /// <inheritdoc cref="Bgt_Un"/>
    GreaterThanUnordered = Bgt_Un,

    /// <summary>
    /// Transfers control to a target instruction if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values.
    /// </summary>
    Ble_Un,

    /// <inheritdoc cref="Ble_Un"/>
    LessOrEqualUnordered = Ble_Un,

    /// <summary>
    /// Transfers control to a target instruction if the first value is less than the second value, when comparing unsigned integer values or unordered float values.
    /// </summary>
    Blt_Un,

    /// <inheritdoc cref="Blt_Un"/>
    LessThanUnordered = Blt_Un,
}
